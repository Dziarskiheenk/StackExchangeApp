using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication5.Handlers;
using WebApplication5.HttpServices;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
  [Route("api/[controller]")]
  public class StackExchangeController : Controller
  {
    private readonly StackExchangeService stackExchangeService;

    public StackExchangeController(StackExchangeService stackExchangeService)
    {
      this.stackExchangeService = stackExchangeService;
    }

    /// <summary>
    /// List of 1000 most popular Stack Overflow tags, sorted by popularity.
    /// </summary>
    /// <returns></returns>
    [HttpGet("[action]")]
    [TypeFilter(typeof(CustomExceptionHandler))]
    [ResponseCache(Duration = 90)] // to avoid api overthrottling
    public async Task<IActionResult> Tags()
    {
      List<Tag> tags = await stackExchangeService.GetTags();
      return Ok(tags);
    }
  }
}
