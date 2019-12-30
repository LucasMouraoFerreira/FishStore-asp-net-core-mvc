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
		public const string ssUserName = "ssUserName";
		public const string ssUserAddress = "ssUserAddress";
		public const string ssUserCity = "ssUserCity";
		public const string ssUserState = "ssUserState";

		public const string StatusSubmitted = "Submitted";
		public const string StatusInProcess = "Being Prepare";
		public const string StatusReady = "Ready to Post";
		public const string StatusPosted = "Posted";
		public const string StatusCanceled = "Canceled";

		public const string PaymentStatusPending = "Pending";
		public const string PaymentStatusApproved = "Approved";
		public const string PaymentStatusRejected = "Rejected";


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
			double sideUsed = 0.0;
			double weightUsed = 0.0;

			if(weight < 1.0)
			{
				weightUsed = 1.0;
			}
			else if(weight > 29)
			{
				weightUsed = 29.0;
			}
			else
			{
				weightUsed = weight;
			}

			if(squareSideLength > 66.0)
			{
				sideUsed = 66.0;
			}else if(squareSideLength < 17.0)
			{
				sideUsed = 17.0;
			}
			else
			{
				sideUsed = squareSideLength;
			}

			string nCdEmpresa = string.Empty;
			string sDsSenha = string.Empty;
			
			string nCdServico = "40010"; // Sedex
			
			string sCepOrigem = "30130010"; // CEP Praça Sete 30130010
			string sCepDestino = cep;

			string nVlPeso = weightUsed.ToString("F0"); // peso em kg da encomenda / peso máximo de 30kg
			int nCdFormato = 1; // formato caixa

			decimal nVlComprimento = (decimal)sideUsed; // medidas em cm entre 16cm e 105cm e a soma das dimensões não deve ultrapassar 200cm
			decimal nVlAltura = (decimal)sideUsed;
			decimal nVlLargura = (decimal)sideUsed;
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
