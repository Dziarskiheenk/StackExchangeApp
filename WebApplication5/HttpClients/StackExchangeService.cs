using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebApplication5.Models;

namespace WebApplication5.HttpServices
{
  public class StackExchangeService
  {
    private readonly HttpClient client;

    public StackExchangeService(HttpClient client)
    {
      this.client = client;
    }

    private async Task<TagsResponse> GetTagsResponse(int page)
    {
      HttpResponseMessage response = await client.GetAsync($"/2.2/tags?page={page}&pagesize=100&order=desc&sort=popular&site=stackoverflow&filter=!-.G.68pp778y");
      response.EnsureSuccessStatusCode();
      string responseContent = await response.Content.ReadAsStringAsync();
      TagsResponse tagsResponse = JsonConvert.DeserializeObject<TagsResponse>(responseContent);
      if (tagsResponse.Items?.Count != 100)
        throw new HttpRequestException("Wrong tags count in API response.");
      return tagsResponse;
    }

    public async Task<List<Tag>> GetTags()
    {
      object locker = new object();
      List<Tag> tags = new List<Tag>();
      IEnumerable<int> pages = Enumerable.Range(1, 10);
      // 100 tags per one request make 10 requests (pages) to get 1000
      var tasks = pages.Select(async page =>
      {
        TagsResponse tagsResponse = await GetTagsResponse(page);
        lock (locker)
          tags.AddRange(tagsResponse.Items);
      });
      await Task.WhenAll(tasks);

      tags.AsParallel().ForAll(tag => { tag.PopularityPercentage = Math.Round(((decimal)tag.Count / tags.Sum(x => x.Count) * 100), 5); });
      tags = tags.OrderByDescending(x => x.Count).ToList();

      return tags;
    }
  }
}
