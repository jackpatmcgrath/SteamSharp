﻿using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace SteamSharp {

	/// <summary>
	/// (Interface) Container for data used to make Steam API requests.
	/// </summary>
	public interface ISteamRequest {

		/// <summary>
		/// Container of all parameters needing to be passed to the Steam API.
		/// See AddParameter() for adding additional parameters to the request.
		/// </summary>
		List<SteamRequestParameter> Parameters { get; }

		/// <summary>
		/// Container for cookies to be added to the HTTP request. To add cookies use AddCookie( Cookie ).
		/// </summary>
		List<Cookie> Cookies { get; }

		/// <summary>
		/// Indicates the standard HTTP method that should be used for this request.
		/// This value defaults to GET.
		/// </summary>
		HttpMethod Method { get; set; }

		/// <summary>
		/// Data Format to use for POST Requests.
		/// </summary>
		PostDataFormat DataFormat { get; set; }

		/// <summary>
		/// The URI the request will be made against.
		/// </summary>
		string Resource { get; set; }

		/// <summary>
		/// Timeout in milliseconds to be used for the request. If this time is exceeded the request will fail.
		/// If not set, defaults to Timeout value in the SteamClient executing this request.
		/// </summary>
		int Timeout { get; set; }

		/// <summary>
		/// Steam API Interface to access (i.e. ISteamNews)
		/// </summary>
		SteamAPIInterface SteamInterface { get; }

		/// <summary>
		/// Method within the Steam API to use (i.e. GetNewsForApp)
		/// </summary>
		string SteamApiMethod { get; }
		/// <summary>
		/// Version of the API being requested (i.e. v0001)
		/// </summary>
		SteamMethodVersion SteamMethodVersion { get; }

		/// <summary>
		/// Serializes object obj into JSON, which is then used as the Body of the HTTP request.
		/// </summary>
		/// <param name="obj">Object to be serialized and used as the Body of the HTTP request.</param>
		/// <returns>This request</returns>
		ISteamRequest AddBody( object obj );

		/// <summary>
		/// Registers a URL Segement with the request. This will replace {name} with value in the specified Resource.
		/// </summary>
		/// <param name="name">Name of the segement to register.</param>
		/// <param name="value">Value to replace the named segement with.</param>
		/// <returns>This request.</returns>
		ISteamRequest AddUrlSegment( string name, string value );

		/// <summary>
		/// Adds an custom HTTP Header to the request.
		/// </summary>
		/// <param name="name">The name of the header (i.e. X-CustomHeader)</param>
		/// <param name="value">The value of the custom header</param>
		/// <returns>This request</returns>
		ISteamRequest AddHeader( string name, string value );

		/// <summary>
		/// Adds a parameter to the request.
		/// </summary>
		/// <param name="param"><see cref="SteamRequestParameter"/> to add to the request.</param>
		/// <returns>This request</returns>
		ISteamRequest AddParameter( SteamRequestParameter param );

		/// <summary>
		/// Adds a parameter to the request.
		/// </summary>
		/// <param name="name">Name of the parameter</param>
		/// <param name="value">Value of the parameter</param>
		/// <returns>This request</returns>
		ISteamRequest AddParameter( string name, object value );

		/// <summary>
		/// Adds a parameter to the request. 
		/// </summary>
		/// <param name="name">Name of the parameter</param>
		/// <param name="value">Value of the parameter</param>
		/// <param name="type">The type of the parameter</param>
		/// <returns>This request</returns>
		ISteamRequest AddParameter( string name, object value, ParameterType type, bool isUrlEncoded = false );

		/// <summary>
		/// Adds a <see cref="Cookie"/> to the request.
		/// </summary>
		/// <param name="cookie"><see cref="Cookie"/> to be added to the request.</param>
		/// <returns>This request</returns>
		ISteamRequest AddCookie( Cookie cookie );

		/// <summary>
		/// Provides the number of times this particular request has been attempted (regardless of success).
		/// </summary>
		int Attempts { get; }

		/// <summary>
		/// Method that allows the request's attempt count to be incremented.
		/// Should only be called by the SteamClient doing an execution operation.
		/// </summary>
		void IncreaseNumAttempts();

	}

}
