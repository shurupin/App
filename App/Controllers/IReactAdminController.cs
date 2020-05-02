namespace App.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IReactAdminController<T>
    {
        Task<ActionResult<IEnumerable<T>>> Get(string filter = "", string range = "", string sort = "");
        Task<ActionResult<T>> Get(long id);
        Task<IActionResult> Put(long id, T entity);
        Task<ActionResult<T>> Post(T entity);
        Task<ActionResult<T>> Delete(long id);
    }
}
