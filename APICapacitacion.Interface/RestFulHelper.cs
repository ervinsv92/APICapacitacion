using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace APICapacitacion.Clases
{
    public static class RestFulHelper
    {
        public static double GetMethodCall()
        {
            double tipoCambioCompra = 0;
            var client = new RestClient("https://gee.bccr.fi.cr/");
            //client.Authenticator = new HttpBasicAuthenticator("username", "password");
            string url = "Indicadores/Suscripciones/WS/wsindicadoreseconomicos.asmx/ObtenerIndicadoresEconomicosXML?Indicador=317&FechaInicio=" + DateTime.Now.ToString("dd/MM/yyyy") + "&FechaFinal=" + DateTime.Now.ToString("dd/MM/yyyy") + "&Nombre=Ervin&SubNiveles=N&CorreoElectronico=ervinsv92@gmail.com&Token=NA9EASRGM1";

            var request = new RestRequest(url, DataFormat.Xml);

            //tipoCambioVenta = new RestRequest(request, CancellationToken.None);
            var response = client.Get(request);
            string xmlString = response.Content;
            xmlString = xmlString.Replace("&lt;", "<");
            xmlString = xmlString.Replace("&gt;", ">");
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmlString);

            var tipo = xml.GetElementsByTagName("string");
            string strXml = tipo[0].InnerXml;
            xml.LoadXml(strXml);

            tipo = xml.GetElementsByTagName("Datos_de_INGC011_CAT_INDICADORECONOMIC");
            strXml = tipo[0].InnerXml;
            xml.LoadXml(strXml);

            tipo = xml.GetElementsByTagName("INGC011_CAT_INDICADORECONOMIC");

            foreach (XmlNode node in tipo[0]) {
                if (node.Name == "NUM_VALOR") {
                    tipoCambioCompra = Double.Parse(node.InnerText.Replace(".", ","));
                }
            }
            //xml.LoadXml(strXml);


            //var tipo = xml.GetElementsByTagName("string/Datos_de_INGC011_CAT_INDICADORECONOMIC/INGC011_CAT_INDICADORECONOMIC/NUM_VALOR");

            return tipoCambioCompra;
        }
        public static async Task<double> GetMethodCall_Respaldo() {

            double tipoCambioVenta = 0;
            var client = new RestClient("https://gee.bccr.fi.cr");
            //client.Authenticator = new HttpBasicAuthenticator("username", "password");
            string url = "/Indicadores/Suscripciones/WS/wsindicadoreseconomicos.asmx/ObtenerIndicadoresEconomicosXML?Indicador=317&FechaInicio=" + DateTime.Now.ToString("yyyy-MM-dd") + "&FechaFinal=" + DateTime.Now.ToString("yyyy-MM-dd") + "&Nombre=Ervin&SubNiveles=N&CorreoElectronico=ervinsv92@gmail.com&Token=NA9EASRGM1";

            var request = new RestRequest(url, DataFormat.None);

            tipoCambioVenta = await client.GetAsync<Double>(request, CancellationToken.None);

            return tipoCambioVenta;
        }

    }
}
