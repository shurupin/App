namespace App.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Threading.Tasks;
    using App.DataLayer.DbContext;

    [Route("api/[controller]")]
    [ApiController]
    public abstract class ReactAdminController<T> : ControllerBase, IReactAdminController<T> where T : class, new()
    {
        protected readonly DefaultDbContext _context;
        protected DbSet<T> _table = null!;

        public ReactAdminController(DefaultDbContext context)
        {
            this._context = context;
        }

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult<T>> Delete(long id)
        {
            T entity = await this._table.FindAsync(id);
            if (entity == null)
            {
                return this.NotFound();
            }

            this._table.Remove(entity);
            await this._context.SaveChangesAsync();

            return this.Ok(entity);
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<T>>> Get(string filter = "", string range = "", string sort = "")
        {
            IQueryable<T> entityQuery = this._table.AsQueryable();

            if (!string.IsNullOrEmpty(filter))
            {
                JObject filterVal = (JObject)JsonConvert.DeserializeObject(filter);
                T t = new T();
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
            this.Response.Headers.Add("Content-Range", $"{typeof(T).Name.ToLower()} {from}-{to}/{count}");
            List<T> entityList = await entityQuery.ToListAsync();
            return entityList;
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<T>> Get(long id)
        {
            T entity = await this._table.FindAsync(id);

            if (entity == null)
            {
                return this.NotFound();
            }

            return entity;
        }

        [HttpPost]
        public virtual async Task<ActionResult<T>> Post(T entity)
        {
            this._table.Add(entity);
            await this._context.SaveChangesAsync();
            long id = (long)typeof(T).GetProperty("Id").GetValue(entity);
            return this.Ok(await this._table.FindAsync(id));
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Put(long id, T entity)
        {
            long entityId = (long)typeof(T).GetProperty("Id").GetValue(entity);
            if (id != entityId)
            {
                return this.BadRequest();
            }

            this._context.Entry(entity).State = EntityState.Modified;

            try
            {
                await this._context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.EntityExists(id))
                {
                    return this.NotFound();
                }
                else
                {
                    throw;
                }
            }

            return this.Ok(await this._table.FindAsync(entityId));
        }

        private bool EntityExists(long id)
        {
            return this._table.Any(entity => (long)typeof(T).GetProperty("Id").GetValue(entity) == id);
        }

    }
}