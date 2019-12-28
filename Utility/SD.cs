using ServiceReferenceCorreios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;


namespace FishStore.Utility
{
	public static class SD
	{
		public const string DefaultImage = "Default_Image.jpg";
		public const string ManagerUser = "Manager";
		public const string CustomerEndUser = "Customer";
		public const string ssPostalCode = "ssPostalCode";

		public static string ConvertToRawHtml(string source)
		{
			char[] array = new char[source.Length];
			int arrayIndex = 0;
			bool inside = false;

			for (int i = 0; i < source.Length; i++)
			{
				char let = source[i];
				if (let == '<')
				{
					inside = true;
					continue;
				}
				if (let == '>')
				{
					inside = false;
					continue;
				}
				if (!inside)
				{
					array[arrayIndex] = let;
					arrayIndex++;
				}
			}
			return new string(array, 0, arrayIndex);
		}

		public static async Task<string[]> GetPriceAndTimePostalServiceAsync(string cep, double weight, double volume)
		{
			double squareSideLength = Math.Pow(volume, 1.0 / 3.0); 

			string nCdEmpresa = string.Empty;
			string sDsSenha = string.Empty;
			
			string nCdServico = "40010"; // Sedex
			
			string sCepOrigem = "05650001"; // CEP Praça Sete 30130010
			string sCepDestino = cep;

			string nVlPeso = "3";// weight.ToString("F0"); // peso em kg da encomenda
			int nCdFormato = 1; // formato caixa

			decimal nVlComprimento = 20; // (decimal)squareSideLength;
			decimal nVlAltura = 20; // (decimal)squareSideLength;
			decimal nVlLargura = 20; // (decimal)squareSideLength;
			decimal nVlDiamentro = 0;
			
			string sCdMaoPropria = "N";

			decimal nVlValorDeclarado = 0;

			string sCdAvisoRecebimento = "N";

			System.ServiceModel.Channels.Binding binding = new BasicHttpBinding();
			EndpointAddress remoteAddress = new EndpointAddress("http://ws.correios.com.br/calculador/CalcPrecoPrazo.asmx");

			CalcPrecoPrazoWSSoapClient wsCorreios2 = new CalcPrecoPrazoWSSoapClient(binding,remoteAddress);

			cResultado retornoCorreios = await wsCorreios2.CalcPrecoPrazoAsync(nCdEmpresa, sDsSenha, nCdServico, sCepOrigem, sCepDestino, nVlPeso, nCdFormato,
				nVlComprimento, nVlAltura, nVlLargura, nVlDiamentro, sCdMaoPropria, nVlValorDeclarado, sCdAvisoRecebimento);

			string[] result = new string[2];
			result[1] = retornoCorreios.Servicos.FirstOrDefault().PrazoEntrega;
			result[0] = retornoCorreios.Servicos.FirstOrDefault().Valor;

			return result;
		}
	}
}
