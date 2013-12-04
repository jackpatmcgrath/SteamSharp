﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteamSharp.Test.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace SteamSharp.Test {

	[TestClass]
	public class UriBuilderTests {

		[TestMethod]
		[TestCategory( "URI - Resource" )]
		public void GET_As_Default() {

			var request = new SteamRequest( "/resource" );
			Assert.AreEqual( request.Method, HttpMethod.Get );

		}

		[TestMethod]
		[TestCategory( "URI - Resource" )]
		public void GET_With_Leading_Slash() {

			var request = new SteamRequest( "/resource" );
			var client = new SteamClient( "http://steamapiurl.com/" );

			var expected = new Uri( "http://steamapiurl.com/resource" );
			var output = client.BuildUri( request );

			Assert.AreEqual( expected, output );

		}

		[TestMethod]
		[TestCategory( "URI - Resource" )]
		public void GET_Without_Leading_Slash() {

			var request = new SteamRequest( "resource" );
			var client = new SteamClient( "http://steamapiurl.com/" );

			var expected = new Uri( "http://steamapiurl.com/resource" );
			var output = client.BuildUri( request );

			Assert.AreEqual( expected, output );

		}

		[TestMethod]
		[TestCategory( "URI - Resource" )]
		public void GET_With_NoSlash_Base_And_Resource_Leading_Slash() {

			var request = new SteamRequest( "/resource" );
			var client = new SteamClient( "http://iforgottoslash.com" );

			var expected = new Uri( "http://iforgottoslash.com/resource" );
			var output = client.BuildUri( request );

			Assert.AreEqual( expected, output );

		}

		[TestMethod]
		[TestCategory( "URI - Resource" )]
		public void GET_With_NoSlash_Base_And_No_Leading_Slash() {

			var request = new SteamRequest( "resource" );
			var client = new SteamClient( "http://iforgottoslash.com" );

			var expected = new Uri( "http://iforgottoslash.com/resource" );
			var output = client.BuildUri( request );

			Assert.AreEqual( expected, output );

		}

		[TestMethod]
		[TestCategory( "URI - Resource" )]
		public void Detect_Malformed_BaseApi() {

			var request = new SteamRequest( "resource" );

			var client = new SteamClient(  "Definitely isn't a URI... How sad :(" );

			AssertException.Throws<FormatException>( () => { client.BuildUri( request ); } );

		}

		/// <summary>
		/// Scenario: User adds two parameters with the same name and type
		/// Expected: The most recently added parameter is honored
		/// </summary>
		[TestMethod]
		[TestCategory( "URI - Parameters" )]
		public void Add_Two_Of_Same_Parameter() {

			var request = new SteamRequest( "/resource" );
			request.AddParameter( "MyFancyParam", 1234, ParameterType.GetOrPost );
			request.AddParameter( "MyFancyParam", 5678, ParameterType.GetOrPost );

			Assert.AreEqual( request.Parameters.Count( p => p.Name == "MyFancyParam" ), 1 );
			Assert.AreEqual( request.Parameters.FirstOrDefault( p => p.Name == "MyFancyParam" ).Value, 5678 );

		}

		[TestMethod]
		[TestCategory( "URI - Parameters" )]
		public void GET_With_Resource_Containing_Tokens() {

			var request = new SteamRequest( "resource/{foo}" );
			request.AddUrlSegment( "foo", "bar" );

			var client = new SteamClient( "http://steamapiurl.com/" );

			var expected = new Uri( "http://steamapiurl.com/resource/bar" );
			var output = client.BuildUri( request );

			Assert.AreEqual( expected, output );

		}

		[TestMethod]
		[TestCategory( "URI - Parameters" )]
		public void POST_With_Resource_Containing_Tokens() {
			
			var request = new SteamRequest( "resource/{foo}", HttpMethod.Post );
			request.AddUrlSegment( "foo", "bar" );

			var client = new SteamClient( "http://steamapiurl.com/" );

			var expected = new Uri( "http://steamapiurl.com/resource/bar" );
			var output = client.BuildUri( request );

			Assert.AreEqual( expected, output );

		}

		[TestMethod]
		[TestCategory( "URI - Parameters" )]
		public void GET_Add_QueryString_Params() {

			using( SimulatedServer.Create( ResourceConstants.SimulatedServerUrl, QueryString_Echo ) ) {

				// All Params added, GetOrPost & QueryString, should be present in the result set
				SteamClient client = new SteamClient( ResourceConstants.SimulatedServerUrl );
				var request = new SteamRequest( "/resource" );

				request.AddParameter( "param1", "1234", ParameterType.GetOrPost );
				request.AddParameter( "param2", "5678", ParameterType.QueryString );

				var response = client.Execute( request );

				Assert.IsNotNull( response.Content );

				string[] temp = response.Content.Split( '|' );

				NameValueCollection coll = new NameValueCollection();
				foreach( string s in temp ) {
					var split = s.Split( '=' );
					coll.Add( split[0], split[1] );
				}

				Assert.IsNotNull( coll["param1"] );
				Assert.AreEqual( "1234", coll["param1"] );

				Assert.IsNotNull( coll["param2"] );
				Assert.AreEqual( "5678", coll["param2"] );

			}

		}

		[TestMethod]
		[TestCategory( "URI - Parameters" )]
		public void POST_Add_QueryString_Params_Raw() {

			using( SimulatedServer.Create( ResourceConstants.SimulatedServerUrl, QueryString_Echo ) ) {

				// All Params added, GetOrPost & QueryString, should be present in the result set
				SteamClient client = new SteamClient( ResourceConstants.SimulatedServerUrl );
				var request = new SteamRequest( "/resource", HttpMethod.Post );
				request.DataFormat = PostDataFormat.Raw;

				request.AddParameter( "param1", "1234", ParameterType.GetOrPost );
				request.AddParameter( "param2", "5678", ParameterType.QueryString );

				var response = client.Execute( request );

				Assert.IsNotNull( response.Content );

				string[] temp = response.Content.Split( '|' );

				NameValueCollection coll = new NameValueCollection();
				foreach( string s in temp ) {
					var split = s.Split( '=' );
					coll.Add( split[0], split[1] );
				}

				Assert.IsNotNull( coll["param1"] );
				Assert.AreEqual( "1234", coll["param1"] );

				Assert.IsNotNull( coll["param2"] );
				Assert.AreEqual( "5678", coll["param2"] );

			}

		}

		[TestMethod]
		[TestCategory( "URI - Parameters" )]
		public void POST_Add_QueryString_Params_Json() {

			using( SimulatedServer.Create( ResourceConstants.SimulatedServerUrl, QueryString_Echo ) ) {

				// Query String params should be in the URI, GetOrPost should not
				SteamClient client = new SteamClient( ResourceConstants.SimulatedServerUrl );
				var request = new SteamRequest( "/resource", HttpMethod.Post );
				request.DataFormat = PostDataFormat.Json;

				request.AddParameter( "param1", "1234", ParameterType.GetOrPost );
				request.AddParameter( "param2", "5678", ParameterType.QueryString );

				var response = client.Execute( request );

				Assert.IsNotNull( response.Content );

				string[] temp = response.Content.Split( '|' );

				NameValueCollection coll = new NameValueCollection();
				foreach( string s in temp ) {
					var split = s.Split( '=' );
					coll.Add( split[0], split[1] );
				}

				Assert.IsNull( coll["param1"] );

				Assert.IsNotNull( coll["param2"] );
				Assert.AreEqual( "5678", coll["param2"] );

			}

		}

		private void QueryString_Echo( HttpListenerContext context ) {

			var data = context.Request;

			Uri requestUri = context.Request.Url;
			var queryParams = HttpUtility.ParseQueryString( requestUri.Query );

			List<string> temp = new List<string>();
			foreach( string key in queryParams.AllKeys ) {
				temp.Add( String.Format( "{0}={1}", key, queryParams[key] ) );
			}

			context.Response.OutputStream.WriteStringUTF8( String.Join( "|", temp ) );
			
		}

	}

}
