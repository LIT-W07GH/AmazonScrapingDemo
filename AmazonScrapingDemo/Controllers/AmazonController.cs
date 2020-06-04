using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AmazonScrapingDemo.Scraping;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AmazonScrapingDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmazonController : ControllerBase
    {
        [HttpGet]
        [Route("search/{term}")]
        public List<AmazonResult> Search(string term)
        {
            return AmazonScraper.Search(term);
        }
    }
}