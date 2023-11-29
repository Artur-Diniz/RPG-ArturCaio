using System.Net.Http.Headers;
using RpgMvc.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace RpgMvc.Controllers
{
    public class PersonagemHabilidadesController : Controller
    {
        public string uriBase = "http://arturdiniz2023.somee.com/RpgApi/PersonagemHabilidades/";

        [HttpGet("PersonagemHabilidades/{id}")]
        public async Task<ActionResult> IndexAsync(int id)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string token = HttpContext.Session.GetString("SessionTokenUsuario");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await httpClient.GetAsync(uriBase + id.ToString());
                string serialized = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    List<PersonagemHabilidadeViewModel> lista = await Task.Run(() =>
                    JsonConvert.DeserializeObject<List<PersonagemHabilidadeViewModel>>(serialized));
                    return View(lista);
                }
                else
                    throw new System.Exception(serialized);
            }
            catch (System.Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpGet("Delete/{Habilidades}/{personagensId}")]
        public async Task<ActionResult> DeleteAsync(int HabilidadeId, int personagensId)
        {

            try
            {
                HttpClient httpClient = new HttpClient();
                string uriComplementar = "DeletePersonagemHabilidade";
                string token = HttpContext.Session.GetString("SessionTokenUsuario");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                PersonagemHabilidadeViewModel ph = new PersonagemHabilidadeViewModel();
                ph.HabilidadeId = HabilidadeId;
                ph.PersonagemId = personagensId;

                var content = new StringContent(JsonConvert.SerializeObject(ph));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await httpClient.PostAsync(uriBase + uriComplementar, content);
                string serialized = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    TempData["Mensagem"] = "Habilidade Removida";
                }
                else
                    throw new System.Exception(serialized);

            }
            catch (System.Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;

            }
            return RedirectToAction("Index", new { Id = personagensId });
        }

        [HttpGet]
        public async Task<ActionResult> CreateAsync(int id, string nome)
        {

            try
            {
                string uriComplementar = "GetHabilidades";
                HttpClient httpClient = new HttpClient();
                string token = HttpContext.Session.GetString("SessionTokenUsuario");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await httpClient.GetAsync(uriBase + uriComplementar);

                string serialized = await response.Content.ReadAsStringAsync();
                List<HabilidadeViewModel> habilidades = await Task.Run(() =>
                JsonConvert.DeserializeObject<List<HabilidadeViewModel>>(serialized));
                ViewBag.ListaHabilidades = habilidades;

                PersonagemHabilidadeViewModel ph = new PersonagemHabilidadeViewModel();
                ph.Personagem = new PersonagemViewModel();
                ph.Habilidade = new HabilidadeViewModel();
                ph.PersonagemId = id;
                ph.Personagem.Nome = nome;

                return View(ph);

            }
            catch (System.Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                return RedirectToAction("Create", new { Id = nome });
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreteAsync(PersonagemHabilidadeViewModel ph)
        {

            try
            {
                HttpClient httpClient = new HttpClient();
                string token = HttpContext.Session.GetString("SessionTokenUsuario");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var content = new StringContent(JsonConvert.SerializeObject(ph));
                content.Headers.ContentType = new MediaTypeHeaderValue("aplication/json");
                HttpResponseMessage response = await httpClient.PostAsync(uriBase, content);
                string serialized = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    TempData["Mensagem"] = "Habilidae cadastrada com sucesso";
                }
                else 
                throw new System.Exception(serialized);

            }
             catch (System.Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                
            }
            return RedirectToAction("Index", new { id = ph.PersonagemId });
        }

    }
}