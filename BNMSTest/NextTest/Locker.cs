using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NextTest
{
    class LockerV1
    {
        private ReaderWriterLock rwLock = new ReaderWriterLock();

        public void ReadMessage()
        {
            rwLock.AcquireReaderLock(Timeout.Infinite);
            try
            {
                Console.WriteLine("Received message");
            }
            finally
            {
                rwLock.ReleaseReaderLock();
            }
        }

        public void WriteMessage()
        {
            rwLock.AcquireWriterLock(Timeout.Infinite);
            try
            {
                Console.WriteLine("Wrote message");
            }
            finally
            {
                rwLock.ReleaseWriterLock();
            }
        }
    }

    class LockerV2
    {
        private ReaderWriterLockSlim rwLockSlim = new ReaderWriterLockSlim();

        public void ReadMessage()
        {
            rwLockSlim.EnterReadLock();
            try
            {
                Console.WriteLine("Received message");
            }
            finally
            {
                rwLockSlim.ExitReadLock();
            }
        }

        public void WriteMessage()
        {
            rwLockSlim.EnterWriteLock();
            try
            {
                Console.WriteLine("Wrote message");
            }
            finally
            {
                rwLockSlim.ExitWriteLock();
            }
        }
    }
}
