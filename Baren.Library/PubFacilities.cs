using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Baren.Library
{
    class PubFacilities
    {
        BlockingCollection<IAgent> agents = new BlockingCollection<IAgent>();
        BlockingCollection<Glass> tables = new BlockingCollection<Glass>(); 
        BlockingCollection<Glass> shelves = new BlockingCollection<Glass>();
        ConcurrentQueue<IAgent> chairs = new ConcurrentQueue<IAgent>();
        ConcurrentQueue<IAgent> bar = new ConcurrentQueue<IAgent>();
        
    }
}
