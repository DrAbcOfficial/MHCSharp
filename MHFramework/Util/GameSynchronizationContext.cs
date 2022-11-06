using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHFramework.Util;

public class GameSynchronizationContext : SynchronizationContext
{
    public required Thread CurrentThread;
    private const int kAwqInitialCapacity = 20;
    private readonly List<WorkRequest> m_AsyncWorkQueue = new List<WorkRequest>();
    private readonly List<WorkRequest> m_CurrentFrameWork = new List<WorkRequest>(kAwqInitialCapacity);
    public override void Post(SendOrPostCallback callback, object? state)
    {
        lock (m_AsyncWorkQueue)
        {
            m_AsyncWorkQueue.Add(new WorkRequest(callback, state));
        }
    }

    public override void Send(SendOrPostCallback callback, object? state)
    {
        if (CurrentThread.ManagedThreadId == Thread.CurrentThread.ManagedThreadId)
        {
            callback(state);
        }
        else
        {
            using (var waitHandle = new ManualResetEvent(false))
            {
                lock (m_AsyncWorkQueue)
                {
                    m_AsyncWorkQueue.Add(new WorkRequest(callback, state, waitHandle));
                }
                waitHandle.WaitOne();
            }
        }
    }

    /// <summary>
    /// 主循环调用
    /// </summary>
    /// <param name="deltaTime"></param>
    public void Update()
    {
        // 锁住
        lock (m_AsyncWorkQueue)
        {
            m_CurrentFrameWork.AddRange(m_AsyncWorkQueue);
            m_AsyncWorkQueue.Clear();
        }

        // When you invoke work, remove it from the list to stop it being triggered again (case 1213602)
        while (m_CurrentFrameWork.Count > 0)
        {
            WorkRequest work = m_CurrentFrameWork[0];
            m_CurrentFrameWork.RemoveAt(0);
            work.Invoke();
        }
    }

}

struct WorkRequest
{
    private readonly SendOrPostCallback m_DelagateCallback;
    private readonly object? m_DelagateState;
    private readonly ManualResetEvent? m_WaitHandle;

    public WorkRequest(SendOrPostCallback callback, object? state, ManualResetEvent? waitHandle = null)
    {
        m_DelagateCallback = callback;
        m_DelagateState = state;
        m_WaitHandle = waitHandle;
    }

    public void Invoke()
    {
        try
        {
            m_DelagateCallback(m_DelagateState);
        }
        finally
        {
            m_WaitHandle?.Set();
        }
    }
}