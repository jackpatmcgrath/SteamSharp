﻿using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SteamSharp {

	/// <summary>
	/// Class allowing for abstracted querying of the IPlayerService interface
	/// </summary>
	public partial class PlayerService : SteamInterface {

		#region GetOwnedGames
		/// <summary>
		/// Returns a list of games a player owns along with some playtime information, if the profile is publicly visible.
		/// Throws <see cref="SteamRequestException"/> on failure.
		/// <a href="https://developer.valvesoftware.com/wiki/Steam_Web_API#GetOwnedGames_.28v0001.29">See official documentation.</a>
		/// </summary>
		/// <param name="client"><see cref="SteamClient"/> instance to use.</param>
		/// <param name="steamID">SteamID to return friend's list for.</param>
		/// <param name="getAppInfo">Include game name and logo information in the output?</param>
		/// <param name="getPlayedFreeGames">By default, free games are excluded (as technically everyone owns them). If flag is true, all games the user has played at some point will be returned.</param>
		/// <returns><see cref="OwnedGames"/> object containing information about the specified user's game collection.</returns>
		public static OwnedGames GetOwnedGames( SteamClient client, string steamID, bool getAppInfo = true, bool getPlayedFreeGames = true ) {
			try {
				return GetOwnedGamesAsync( client, steamID, getAppInfo, getPlayedFreeGames ).Result;
			} catch( AggregateException e ) {
				if( e.InnerException != null )
					throw e.InnerException;
				throw e;
			}
		}

		/// <summary>
		/// (Async) Returns a list of games a player owns along with some playtime information, if the profile is publicly visible.
		/// Throws <see cref="SteamRequestException"/> on failure.
		/// <a href="https://developer.valvesoftware.com/wiki/Steam_Web_API#GetOwnedGames_.28v0001.29">See official documentation.</a>
		/// </summary>
		/// <param name="client"><see cref="SteamClient"/> instance to use.</param>
		/// <param name="steamID">SteamID to return friend's list for.</param>
		/// <param name="getAppInfo">Include game name and logo information in the output?</param>
		/// <param name="getPlayedFreeGames">By default, free games are excluded (as technically everyone owns them). If flag is true, all games the user has played at some point will be returned.</param>
		/// <returns><see cref="OwnedGames"/> object containing information about the specified user's game collection.</returns>
		public async static Task<OwnedGames> GetOwnedGamesAsync( SteamClient client, string steamID, bool getAppInfo = true, bool getPlayedFreeGames = true ) {

			SteamRequest request = new SteamRequest( SteamAPIInterface.IPlayerService, "GetOwnedGames", SteamMethodVersion.v0001 );
			request.AddParameter( "steamid", steamID, ParameterType.QueryString );
			request.AddParameter( "include_appinfo", ( ( getAppInfo ) ? 1 : 0 ), ParameterType.QueryString );
			request.AddParameter( "include_played_free_games", ( ( getPlayedFreeGames ) ? 1 : 0 ), ParameterType.QueryString );

			return VerifyAndDeserialize<GetOwnedGamesResponse>( ( await client.ExecuteAsync( request ) ) ).OwnedGames;

		}
		#endregion

		#region GetRecentlyPlayedGames
		/// <summary>
		/// Returns a list of games a player has played in the last two weeks.
		/// Throws <see cref="SteamRequestException"/> on failure.
		/// <a href="https://developer.valvesoftware.com/wiki/Steam_Web_API#GetRecentlyPlayedGames_.28v0001.29">See official documentation.</a>
		/// </summary>
		/// <param name="client"><see cref="SteamClient"/> instance to use.</param>
		/// <param name="steamID">SteamID to return friend's list for.</param>
		/// <param name="maxSelect">Optionally limit response to a certain number of games. Defaults to -1, meaning no limit is imposed.</param>
		/// <returns><see cref="OwnedGames"/> object containing information about the specified user's game collection.</returns>
		public static PlayedGames GetRecentlyPlayedGames( SteamClient client, string steamID, int maxSelect = -1 ) {
			try {
				return GetRecentlyPlayedGamesAsync( client, steamID, maxSelect ).Result;
			} catch( AggregateException e ) {
				if( e.InnerException != null )
					throw e.InnerException;
				throw e;
			}
		}

		/// <summary>
		/// (Async) Returns a list of games a player has played in the last two weeks.
		/// Throws <see cref="SteamRequestException"/> on failure.
		/// <a href="https://developer.valvesoftware.com/wiki/Steam_Web_API#GetRecentlyPlayedGames_.28v0001.29">See official documentation.</a>
		/// </summary>
		/// <param name="client"><see cref="SteamClient"/> instance to use.</param>
		/// <param name="steamID">SteamID to return friend's list for.</param>
		/// <param name="maxSelect">Optionally limit response to a certain number of games. Defaults to -1, meaning no limit is imposed.</param>
		/// <returns><see cref="OwnedGames"/> object containing information about the specified user's game collection.</returns>
		public async static Task<PlayedGames> GetRecentlyPlayedGamesAsync( SteamClient client, string steamID, int maxSelect = -1 ) {

			SteamRequest request = new SteamRequest( SteamAPIInterface.IPlayerService, "GetRecentlyPlayedGames", SteamMethodVersion.v0001 );
			request.AddParameter( "steamid", steamID, ParameterType.QueryString );

			if( maxSelect > 0 ) 
				request.AddParameter( "count", maxSelect, ParameterType.QueryString );

			return VerifyAndDeserialize<GetRecentlyPlayedGamesResponse>( ( await client.ExecuteAsync( request ) ) ).PlayedGames;

		}
		#endregion

	}

}