using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication5.Models
{
  public class Tag
  {
    public string Name { get; set; }
    public int Count { get; set; }
    public decimal? PopularityPercentage { get; set; } = null;
  }

  public class TagsResponse
  {
    public List<Tag> Items { get; set; }
  }

  public class TestClasses
  {
    static Random random = new Random();
    /// <summary>
    /// for debugging purposes only - calling multiple times stack exchange API causes overthrottling
    /// </summary>
    /// <returns></returns>
    public static TagsResponse GetExampleTagsResponse()
    {
      TagsResponse tagsResponse = new TagsResponse() { Items = new List<Tag>() };
      int baseCount = random.Next(1000, 10000);
      for (int i = 0; i < 100; i++)
      {
        tagsResponse.Items.Add(new Tag() { Name = $"tag{i}", Count = baseCount - i });
      }
      return tagsResponse;
    }
  }
}
