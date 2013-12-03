﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SteamSharp {

	public partial class PlayerService {

		#region GetOwnedGames
		/// <summary>
		/// Object for executing the GetOwnedGames API call.
		/// </summary>
		public class GetOwnedGamesResponse {

			/// <summary>
			/// Information about the specified Steam user's games library.
			/// </summary>
			[JsonProperty( "response" )]
			public OwnedGames OwnedGames { get; set; }

		}

		/// <summary>
		/// Information about the specified Steam user's games library.
		/// </summary>
		public class OwnedGames {

			/// <summary>
			/// The total number of games the user owns (including free games they've played, if getPlayedFreeGames is set).
			/// </summary>
			[JsonProperty("game_count")]
			public int GameCount { get; set; }

			/// <summary>
			/// List of <see cref="Game"/>s that are owned by the specified user.
			/// </summary>
			public List<Game> Games { get; set; }

		}
		#endregion

		#region GetRecentlyPlayedGames
		/// <summary>
		/// Object for executing the GetOwnedGames API call.
		/// </summary>
		public class GetRecentlyPlayedGamesResponse {

			/// <summary>
			/// Information about the games the specified Steam user has played recently.
			/// </summary>
			[JsonProperty( "response" )]
			public PlayedGames PlayedGames { get; set; }
		}

		/// <summary>
		/// Information about the games the specified Steam user has played recently.
		/// </summary>
		public class PlayedGames {

			/// <summary>
			/// The total number of unique games the user has played in the last two weeks.
			/// This is mostly significant if you opted to return a limited number of games with the count input parameter.
			/// </summary>
			[JsonProperty( "total_count" )]
			public int TotalCount { get; set; }

			/// <summary>
			/// List of <see cref="Game"/>s that have been recently played by the user.
			/// </summary>
			public List<Game> Games { get; set; }

		}
		#endregion

		#region Shared Models
		/// <summary>
		/// News information for the specified GameID/AppID.
		/// </summary>
		public class Game {

			/// <summary>
			/// Unique ID of the Game (synonymous with AppID).
			/// </summary>
			[JsonProperty( "appid" )]
			public int GameID { get; set; }

			/// <summary>
			/// Name of the game.
			/// </summary>
			public string Name { get; set; }

			/// <summary>
			/// The total number of minutes played "on record", since Steam began tracking total playtime in early 2009.
			/// </summary>
			[JsonProperty( "playtime_forever" )]
			public int PlaytimeForever { get; set; }

			/// <summary>
			/// The total number of minutes played in the last 2 weeks.
			/// </summary>
			[JsonProperty( "playtime_2weeks" )]
			public int PlaytimeTwoWeeks { get; set; }

			/// <summary>
			/// Full URL for the Game's icon image.
			/// </summary>
			[JsonProperty( "img_icon_url" )]
			public string ImgIconURL { get; set; }

			/// <summary>
			/// Fully URL for the Game's logo image.
			/// </summary>
			[JsonProperty( "img_logo_url" )]
			public string ImgLogoURL { get; set; }

			/// <summary>
			/// (Set on GetOwnedGames calls only)
			/// Indicates there is a stats page with achievements or other game stats available for this game.
			/// </summary>
			[JsonProperty( "has_community_visible_stats" )]
			public bool HasCommunityVisibileStats { get; set; }

		}
		#endregion

	}

}