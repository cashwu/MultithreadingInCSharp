using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;

namespace Recipe04
{
    class Program
    {
        static Dictionary<string, string[]> _contentEmulation = new Dictionary<string, string[]>();
        
        static void Main(string[] args)
        {
            CreateLink();
            Task t = RunProgram();
            t.Wait();
        }

        static async Task RunProgram()
        {
            var bag = new ConcurrentBag<CrawlingTask>();
            string[] urls = 
            {
                "http://microsoft.com",
                "http;//google.com",
                "http://facebook.com",
                "http://twitter.com"
            };
            
            var crawlers = new Task[4];

            for (int i = 1; i <= 4; i++)
            {
                string crawlerName = $" Crawler {i}";
                
                bag.Add(new CrawlingTask
                {
                    UrlToCraw = urls[i-1],
                    ProducerName = "root"
                });

                crawlers[i - 1] = Task.Run(() => Crawl(bag, crawlerName));
            }

            await Task.WhenAll(crawlers);
        }

        private static async Task Crawl(ConcurrentBag<CrawlingTask> bag, string crawlerName)
        {
            CrawlingTask task;

            while (bag.TryTake(out task))
            {
                IEnumerable<string> urls = await GetLinksFromContent(task);

                if (urls != null)
                {
                    foreach (var url in urls)
                    {
                        var t = new CrawlingTask
                        {
                            UrlToCraw = url,
                            ProducerName = crawlerName
                        };
                        
                        bag.Add(t);
                    }   
                }

                Console.WriteLine($"Indexing url {task.UrlToCraw} posted by {task.ProducerName} is completed by {crawlerName}");
            }
        }

        private static async Task<IEnumerable<string>> GetLinksFromContent(CrawlingTask task)
        {
            await GetRandomDelay();

            if (_contentEmulation.ContainsKey(task.UrlToCraw))
            {
                return _contentEmulation[task.UrlToCraw];
            }

            return null;
        }

        private static void CreateLink()
        {
            _contentEmulation["http://microsoft.com"] = new[]
            {
                "http://microsoft.com/a.html",
                "http://microsoft.com/b.html"
            };
            
            _contentEmulation["http://microsoft.com/a.html"] = new[]
            {
                "http://microsoft.com/c.html",
                "http://microsoft.com/d.html"
            };
            
            _contentEmulation["http://microsoft.com/b.html"] = new[]
            {
                "http://microsoft.com/e.html"
            };
        }

        private static Task GetRandomDelay()
        {
            int delay = new Random(DateTime.Now.Millisecond).Next(150, 200);
            return Task.Delay(delay);
        }
    }

    internal class CrawlingTask
    {
        public string UrlToCraw { get; set; }
        public string ProducerName { get; set; }
    }
}