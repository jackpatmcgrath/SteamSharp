﻿
namespace SteamSharp.Test {

	public class ResourceConstants {

		/// <summary>
		/// This should be the Steam API Key for use with testing
		/// </summary>
		public const string AccessToken = "1241F1FF2496C1E6990F73E216C3C53D";

		/// <summary>
		/// Address of the simulated web server for handling response call tests.
		/// The only time you will likely need to change this is if there is an active local service on the same port.
		/// </summary>
		public const string SimulatedServerUrl = "http://localhost:8080/";

	}

}
