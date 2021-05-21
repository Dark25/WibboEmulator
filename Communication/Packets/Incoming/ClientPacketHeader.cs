﻿namespace Butterfly.Communication.Packets
{
    public static class ClientPacketHeader //PRODUCTION-201611291003-a-1161735
    {
        public const int WhiperGroupMessageEvent = 1118; //Custom
        public const int CameraPurchaseMessageEvent = 2408; //Custom

        public const int AnswerPollMessageEvent = 3505;
        public const int GetHabboGroupBadgesMessageEvent = 21;
        public const int FindRandomFriendingRoomMessageEvent = 1703;
        public const int GenerateSecretKeyMessageEvent = 773;
        public const int RequestGuideToolEvent = 1922;
        public const int UserNuxEvent = 1299;
        public const int GuideEndSession = 887;
        public const int CancellInviteGuide = 291;
        public const int OnGuideSessionDetached = 1424;
        public const int OnGuide = 3338;
        public const int GuideToolMessageNew = 3899;
        public const int GuideRecommendHelperEvent = 477;
        public const int GuideInviteUserEvent = 234;
        public const int GuideVisitUserEvent = 1052;
        public const int GoToFlatMessageEvent = 685;
        public const int OpenHelpToolMessageEvent = 3267;
        public const int GetGroupFurniSettingsMessageEvent = 2651;
        public const int GetSellablePetBreedsMessageEvent = 1756;
        public const int CheckPetNameMessageEvent = 2109;
        public const int CanCreateRoomMessageEvent = 2128;
        public const int GetPetTrainingPanelMessageEvent = 2161;
        public const int ApplyHorseEffectMessageEvent = 1328;
        public const int RemoveSaddleFromHorseMessageEvent = 186;
        public const int ModifyWhoCanRideHorseMessageEvent = 1472;
        public const int PickTicketMessageEvent = 15;
        public const int CloseTicketMesageEvent = 2067;
        public const int ReleaseTicketMessageEvent = 1572;
        public const int GetQuestListMessageEvent = 3333;
        public const int StartQuestMessageEvent = 3604;
        public const int CancelQuestMessageEvent = 2397;
        public const int GetCurrentQuestMessageEvent = 2750;
        public const int OnBullyClickMessageEvent = 2455;
        public const int SendBullyReportMessageEvent = 3786;
        public const int SubmitBullyReportMessageEvent = 3060;
        public const int GetSanctionStatusMessageEvent = 2746;
        public const int CheckValidNameMessageEvent = 3950;
        public const int ChangeNameMessageEvent = 2977;
        public const int RemoveGroupFavouriteMessageEvent = 1820;
        public const int SetGroupFavouriteMessageEvent = 3549;
        public const int GetOffersMessageEvent = 2407;
        public const int MARKETPLACE_REQUEST_OWN_ITEMS = 2105;
        public const int REQUEST_SELL_ITEM = 848;
        public const int REQUEST_MARKETPLACE_ITEM_STATS = 3288;
        public const int MARKETPLACE_SELL_ITEM = 3447;
        public const int MARKETPLACE_TAKE_BACK_ITEM = 434;
        public const int BuyOfferMessageEvent = 1603;
        public const int MARKETPLACE_REDEEM_CREDITS = 2650;
        public const int FootballGateSaveLookEvent = 924;

        public const int CATALOG_PURCHASE_GIFT = 1411;
        public const int LOVELOCK_START_CONFIRM = 3775;
        public const int CATALOG_REDEEM_VOUCHER = 339;
        public const int PET_RESPECT = 3202;
        public const int PET_MOVE = 3449;
        public const int ROOM_TONER_APPLY = 2880;
        public const int ONE_WAY_DOOR_CLICK = 2765;
        public const int GET_ITEM_DATA = 3964;
        public const int MODIFY_WALL_ITEM_DATA = 3666;
        public const int MODTOOL_REQUEST_USER_ROOMS = 3526;
        public const int MODTOOL_REQUEST_USER_CHATLOG = 1391;
        public const int MODTOOL_SANCTION_KICK = 2582;
        public const int MODTOOL_SANCTION_MUTE = 1945;
        public const int MODTOOL_SANCTION_BAN = 2766;
        public const int MODTOOL_ALERTEVENT = 1840;
        public const int MODTOOL_SANCTION_ALERT = 229;
        public const int MODTOOL_CHANGE_ROOM_SETTINGS = 3260;
        public const int MODTOOL_REQUEST_ROOM_CHATLOG = 2587;
        public const int MODTOOL_REQUEST_ROOM_INFO = 707;
        public const int MODTOOL_ROOM_ALERT = 3842;
        public const int MANNEQUIN_SAVE_NAME = 2850;
        public const int MANNEQUIN_SAVE_LOOK = 2209;
        public const int GROUP_BUY = 230;
        public const int GROUP_PARTS = 813;
        public const int GROUP_SAVE_INFORMATION = 3137;
        public const int GROUP_SAVE_BADGE = 1991;
        public const int GROUP_SAVE_COLORS = 1764;
        public const int GROUP_SAVE_PREFERENCES = 3435;
        public const int GROUP_DELETE = 1134;
        public const int TRADE_UNACCEPT = 1444;
        public const int USER_IGNORED = 3878;
        public const int USER_IGNORE = 1117;
        public const int USER_UNIGNORE = 2061;
        public const int MESSENGER_SEARCH = 1210;
        public const int CATALOG_SEARCH = 2594;
        public const int CATALOG_PURCHASE = 3492;
        public const int GROUP_FORUM_LIST = 873;
        public const int EVENT_TRACKER = 3457;
        public const int CAMERA_PRICE = 796;
        public const int USER_SUBSCRIPTION = 3166;
        public const int USER_CURRENCY = 273;
        public const int USER_SETTINGS_OLD_CHAT = 1262;
        public const int USER_SETTINGS_CAMERA = 1461;
        public const int USER_SETTINGS_INVITES = 1086;
        public const int NAVIGATOR_SEARCH = 249;
        public const int NAVIGATOR_INIT = 2110;
        public const int RELEASE_VERSION = 4000;
        public const int SECURITY_MACHINE = 2490;
        public const int SECURITY_TICKET = 2419;
        public const int USER_INFO = 357;
        public const int DESKTOP_CAMPAIGNS = 2912;
        public const int DESKTOP_NEWS = 1827;
        public const int ROOM_MODEL_SAVE = 875;
        public const int ROOM_MODEL_DOOR = 3559;
        public const int ITEM_STACK_HELPER = 3839;
        public const int USER_SETTINGS_VOLUME = 1367;
        public const int MESSENGER_RELATIONSHIPS = 2138;
        public const int MESSENGER_RELATIONSHIPS_UPDATE = 3768;
        public const int ROOM_MODEL = 2300;
        public const int FURNITURE_ALIASES = 3898;
        public const int ROOM_INFO = 2230;
        public const int CLIENT_LATENCY = 295;
        public const int CLIENT_TOOLBAR_TOGGLE = 2313;
        public const int NAVIGATOR_CATEGORIES = 3027;
        public const int CLIENT_PONG = 2596;
        public const int USER_OUTFITS = 2742;
        public const int USER_OUTFIT_SAVE = 800;
        public const int ACHIEVEMENT_LIST = 219;
        public const int CATALOG_MODE = 1195;
        public const int CATALOG_PAGE = 412;
        public const int GIFT_CONFIG = 418;
        public const int USER_EFFECT_ENABLE = 1752;
        public const int USER_EFFECT_ACTIVATE = 2959;
        public const int ROOM_CREATE = 2752;
        public const int ROOM_SETTINGS = 3129;
        public const int ROOM_SETTINGS_SAVE = 1969;
        public const int ROOM_ENTER = 2312;
        public const int UNIT_LOOK = 3301;
        public const int TRADE = 1481;
        public const int TRADE_ITEMS = 1263;
        public const int TRADE_ITEM = 3107;
        public const int TRADE_ITEM_REMOVE = 3845;
        public const int TRADE_ACCEPT = 3863;
        public const int TRADE_CLOSE = 2551;
        public const int TRADE_CONFIRM = 2760;
        public const int TRADE_CANCEL = 2341;
        public const int UNIT_WALK = 3320;
        public const int UNIT_CHAT = 1314;
        public const int UNIT_CHAT_SHOUT = 2085;
        public const int UNIT_CHAT_WHISPER = 1543;
        public const int USER_RESPECT = 2694;
        public const int USER_FIGURE = 2730;
        public const int USER_MOTTO = 2228;
        public const int UNIT_POSTURE = 2235;
        public const int UNIT_DANCE = 2080;
        public const int UNIT_SIGN = 1975;
        public const int UNIT_ACTION = 2456;
        public const int MESSENGER_REQUESTS = 2448;
        public const int MESSENGER_INIT = 2781;
        public const int MESSENGER_REQUEST = 3157;
        public const int MESSENGER_ACCEPT = 137;
        public const int MESSENGER_DECLINE = 2890;
        public const int MESSENGER_CHAT = 3567;
        public const int USER_FOLLOW = 3997;
        public const int MESSENGER_ROOM_INVITE = 1276;
        public const int MESSENGER_REMOVE = 1689;
        public const int USER_PROFILE = 3265;
        public const int USER_FURNITURE = 3150;
        public const int USER_PETS = 3095;
        public const int USER_BADGES = 2769;
        public const int USER_BOTS = 3848;
        public const int PET_PLACE = 2647;
        public const int PET_PICKUP = 1581;
        public const int PET_INFO = 2934;
        public const int PET_RIDE = 1036;
        public const int ITEM_PAINT = 711;
        public const int FURNITURE_PLACE = 1258;
        public const int FURNITURE_FLOOR_UPDATE = 248;
        public const int FURNITURE_WALL_UPDATE = 168;
        public const int FURNITURE_MULTISTATE = 99;
        public const int FURNITURE_WALL_MULTISTATE = 210;
        public const int FURNITURE_WALL_DELETE = 3336;
        public const int FURNITURE_OPEN_GIFT = 3558;
        public const int ITEM_COLOR_WHEEL_CLICK = 2144;
        public const int ITEM_DIMMER_SETTINGS = 2813;
        public const int ITEM_DIMMER_TOGGLE = 2296;
        public const int ITEM_DIMMER_SAVE = 1648;
        public const int FURNITURE_POSTIT_PLACE = 2248;
        public const int USER_HOME_ROOM = 1740;
        public const int ROOM_DELETE = 532;
        public const int ITEM_EXCHANGE_REDEEM = 3115;
        public const int UNIT_TYPING = 1597;
        public const int UNIT_TYPING_STOP = 1474;
        public const int ROOM_RIGHTS_GIVE = 808;
        public const int ROOM_RIGHTS_LIST = 3385;
        public const int ROOM_BAN_LIST = 2267;
        public const int ROOM_BAN_REMOVE = 992;
        public const int ROOM_RIGHTS_REMOVE_ALL = 2683;
        public const int ROOM_RIGHTS_REMOVE = 2064;
        public const int ROOM_RIGHTS_REMOVE_OWN = 3182;
        public const int FURNITURE_PICKUP = 3456;
        public const int WIRED_TRIGGER_SAVE = 1520;
        public const int WIRED_ACTION_SAVE = 2281;
        public const int WIRED_CONDITION_SAVE = 3203;
        public const int MOD_TOOL_USER_INFO = 3295;
        public const int REPORT = 1691;
        public const int ROOM_LIKE = 3582;
        public const int ROOM_KICK = 1320;
        public const int ROOM_BAN_GIVE = 1477;
        public const int ROOM_MUTE_USER = 3485;
        public const int ROOM_MUTE = 3637;
        public const int ROOM_DOORBELL = 1644;
        public const int ROOM_FAVORITE = 3817;
        public const int ROOM_FAVORITE_REMOVE = 309;
        public const int USER_BADGES_CURRENT_UPDATE = 644;
        public const int USER_BADGES_CURRENT = 2091;
        public const int DESKTOP_VIEW = 105;
        public const int UNIT_DROP_HAND_ITEM = 2814;
        public const int UNIT_GIVE_HANDITEM = 2941;
        public const int ITEM_DICE_CLICK = 1990;
        public const int ITEM_DICE_CLOSE = 1533;
        public const int GROUP_CREATE_OPTIONS = 798;
        public const int GROUP_SETTINGS = 1004;
        public const int GROUP_MEMBERS = 312;
        public const int GROUP_REQUEST = 998;
        public const int GROUP_REQUEST_DECLINE = 1894;
        public const int GROUP_REQUEST_ACCEPT = 3386;
        public const int GROUP_ADMIN_ADD = 2894;
        public const int GROUP_ADMIN_REMOVE = 722;
        public const int GROUP_MEMBERSHIPS = 367;
        public const int GROUP_INFO = 2991;
        public const int GROUP_MEMBER_REMOVE = 3593;
        public const int BOT_PLACE = 1592;
        public const int BOT_PICKUP = 3323;
        public const int BOT_INFO = 1986;
        public const int BOT_SETTINGS_SAVE = 2624;
        public const int ITEM_SAVE_BACKGROUND = 3608;
        public const int FIND_FRIENDS = 516;
        public const int MESSENGER_UPDATES = 1419;
        public const int MARKETPLACE_CONFIG = 2597;
    }
}