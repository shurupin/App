namespace App.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using App.DataLayer.DbContext;
    using App.Models.Core;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Microsoft.EntityFrameworkCore;
    using System.Linq.Dynamic.Core;

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ReactAdminController<User>
    {
        public UserController(DefaultDbContext context) : base(context)
        {
            this._table = context.Users;
        }

        [HttpGet]
        public override async Task<ActionResult<IEnumerable<User>>> Get(string filter = "", string range = "", string sort = "")
        {
            IQueryable<User> entityQuery = this._table.AsQueryable();

            if (entityQuery.Count() == 0)
            {
                entityQuery = new AsyncEnumerable<User>(new List<User>
                {
                    new User {Id = 1, Email = "test@mail.ru", Name = "Name 1",},
                    new User {Id = 2, Email = "test2@mail.ru", Name = "Name 2",}
                }).AsQueryable();
            }

            if (!string.IsNullOrEmpty(filter))
            {
                JObject filterVal = (JObject)JsonConvert.DeserializeObject(filter);
                User t = new User();
                foreach (KeyValuePair<string, JToken> f in filterVal)
                {
                    if (t.GetType().GetProperty(f.Key).PropertyType == typeof(string))
                    {
                        entityQuery = entityQuery.Where($"{f.Key}.Contains(@0)", f.Value.ToString());
                    }
                    else
                    {
                        entityQuery = entityQuery.Where($"{f.Key} == @0", f.Value.ToString());
                    }
                }
            }

            int count = entityQuery.Count();

            if (!string.IsNullOrEmpty(sort))
            {
                List<string> sortVal = JsonConvert.DeserializeObject<List<string>>(sort);
                string condition = sortVal.First();
                string order = sortVal.Last() == "ASC" ? "" : "descending";
                entityQuery = entityQuery.OrderBy($"{condition} {order}");
            }

            int from = 0;
            int to = 0;
            if (!string.IsNullOrEmpty(range))
            {
                List<int> rangeVal = JsonConvert.DeserializeObject<List<int>>(range);
                from = rangeVal.First();
                to = rangeVal.Last();
                entityQuery = entityQuery.Skip(from).Take(to - from + 1);
            }

            this.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Range");
            this.Response.Headers.Add("Content-Range", $"{typeof(User).Name.ToLower()} {from}-{to}/{count}");
            List<User> entityList = await entityQuery.ToListAsync();
            return entityList;
        }
    }
}
