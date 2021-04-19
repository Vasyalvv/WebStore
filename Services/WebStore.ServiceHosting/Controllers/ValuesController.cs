using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebStore.Interfaces;

namespace WebStore.ServiceHosting.Controllers
{
    [Route(WebAPI.TestWebAPI)]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private static readonly List<string> __Values = Enumerable
            .Range(1, 10)
            .Select(i => $"Value-{i:00}")
            .ToList();

        [HttpGet] //Позволяет получить доступ к контроллеру через адрез http://localhost:5001/api/Values
        public IEnumerable<string> Get() => __Values;

        //Вариант метода Get(), позволяющий вернуть статусные коды
        //public ActionResult<IEnumerable<string>> Get() => __Values;

        [HttpGet("{id}")] //Позволяет получить доступ к контроллеру через адрес http://localhost:5001/api/Values/8
        public ActionResult<string> Get(int id)
        {
            if (id < 0)
                return BadRequest();
            if (id >= __Values.Count)
                return NotFound();

            return __Values[id];
        }


        //Добавление объекта
        [HttpPost]          //post -> http://localhost:5001/api/Values
        [HttpPost("add")]   //post -> http://localhost:5001/api/Values/add
        //Можно настраивать откуда будет считано значение передаваемого методу параметра
        //[FromRoute] Адрес должени быть вида http://localhost:5001/api/Values/8
        //[FromQuery] Адрес должени быть вида http://localhost:5001/api/Values?value=213
        //[FromBody] внутри тела запроса
        public ActionResult Post([FromBody] string value)
        {
            __Values.Add(value);

            //return Ok();
            return CreatedAtAction(nameof(Get), new { id = __Values.Count - 1 });
        }


        //Редактирование объекта
        [HttpPut("{id}")]       //put -> http://localhost:5001/api/Values/56
        [HttpPut("edit/{id}")]  //put -> http://localhost:5001/api/Values/edit/56
        public ActionResult Put(int id, [FromBody] string value)
        {
            if (id < 0)
                return BadRequest();
            if (id >= __Values.Count)
                return NotFound();

            __Values[id] = value;
            return Ok();
        }

        //Удаление объекта
        [HttpDelete("{id}")]       //delete -> http://localhost:5001/api/Values/56
        public ActionResult Delete(int id)
        {
            if (id < 0)
                return BadRequest();
            if (id >= __Values.Count)
                return NotFound();

            __Values.RemoveAt(id);

            return Ok();
        }
    }
}
