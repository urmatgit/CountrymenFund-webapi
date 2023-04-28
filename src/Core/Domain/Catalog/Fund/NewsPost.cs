using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Domain.Catalog.Fund;

public  class NewsPost: AuditableEntity, IAggregateRoot
{
    
    public string Title { get; set; }
    public string Author { get; set; }
    public string Body { get; set; }
    public string Images { get; set; }
    public NewsPost()
    {
    
    }

    public NewsPost(string title,string author,string body)
    {
        Title = title;
        Author = author;
        Body = body;

    }
    public void Update(string title, string author, string body)
    {
        Title = title;
        Author = author;
        Body = body;
    }
}
