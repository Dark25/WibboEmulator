﻿namespace Butterfly.Communication.Packets.Outgoing
{
    public static class ServerPacketHeader //PRODUCTION-201611291003-a-1161735
    {
        public const int InitCryptoMessageComposer = 1347;
        public const int SecretKeyMessageComposer = 3885;
        public const int NuxAlertComposer = 1243;
        public const int PetTrainingPanelMessageComposer = 1164;
        public const int CanCreateRoomMessageComposer = 378;
        public const int GroupMembershipRequestedMessageComposer = 1180;
        public const int RefreshFavouriteGroupMessageComposer = 876;

        public const int YOUTUBE_TV = 10003;
        public const int RP_STATS = 10006;
        public const int ADD_CHATLOGS = 10007;
        public const int BUY_ITEMS_LIST = 10008;
        public const int LOAD_INVENTORY_RP = 10009;
        public const int ADD_INVENTORY_ITEM_RP = 10010;
        public const int REMOVE_ITEM_INVENTORY_RP = 10011;
        public const int NOTIF_ALERT = 10012;
        public const int RP_TROC_START = 10013;
        public const int RP_TROC_STOP = 10014;
        public const int RP_TROC_ACCEPTE = 10015;
        public const int RP_TROC_CONFIRME = 10016;
        public const int RP_TROC_UPDATE_ITEMS = 10017;
        public const int NOTIF_TOP = 10018;
        public const int PLAY_SOUND = 10021;
        public const int STOP_SOUND = 10022;
        public const int BOT_CHOOSE = 10023;
        public const int MENTION = 10024;

        public const int ACHIEVEMENT_LIST = 305;
        public const int AUTHENTICATED = 2491;
        public const int AUTHENTICATION = -1;
        public const int AVAILABILITY_STATUS = 2033;
        public const int BUILDERS_CLUB_EXPIRED = 1452;
        public const int CLUB_OFFERS = 2405;
        public const int CATALOG_PAGE = 804;
        public const int CATALOG_PAGE_LIST = 1032;
        public const int CATALOG_PURCHASE_OK = 869;
        public const int CATALOG_PURCHASE_ERROR = 1404;
        public const int CATALOG_PURCHASE_NOT_ALLOWED = 3770;
        public const int PRODUCT_OFFER = 3388;
        public const int LIMITED_SOLD_OUT = 377;
        public const int CATALOG_PUBLISHED = 1866;
        public const int CFH_RESULT_MESSAGE = 3635;
        public const int CLIENT_LATENCY = 10;
        public const int CLIENT_PING = 3928;
        public const int DESKTOP_CAMPAIGN = 1745;
        public const int DESKTOP_NEWS = 286;
        public const int DESKTOP_VIEW = 122;
        public const int BUNDLE_DISCOUNT_RULESET = 2347;
        public const int FIRST_LOGIN_OF_DAY = 793;
        public const int FURNITURE_ALIASES = 1723;
        public const int FURNITURE_DATA = 2547;
        public const int FURNITURE_FLOOR = 1778;
        public const int FURNITURE_FLOOR_ADD = 1534;
        public const int FURNITURE_FLOOR_REMOVE = 2703;
        public const int FURNITURE_FLOOR_UPDATE = 3776;
        public const int FURNITURE_ITEMDATA = 2202;
        public const int FURNITURE_STATE = 2376;
        public const int FURNITURE_GROUP_CONTEXT_MENU_INFO = 3293;
        public const int FURNITURE_POSTIT_STICKY_POLE_OPEN = 2366;
        public const int GAME_CENTER_ACHIEVEMENTS = 2265;
        public const int GAME_CENTER_GAME_LIST = 222;
        public const int GAME_CENTER_STATUS = 2893;
        public const int GENERIC_ALERT = 3801;
        public const int MODERATOR_MESSAGE = 2030;
        public const int GENERIC_ERROR = 1600;
        public const int GIFT_WRAPPER_CONFIG = 2234;
        public const int GROUP_BADGES = 2402;
        public const int GROUP_CREATE_OPTIONS = 2159;
        public const int GROUP_FORUM_INFO = 3011;
        public const int GROUP_FORUM_LIST = 3001;
        public const int GROUP_FORUM_THREADS = 1073;
        public const int GROUP_INFO = 1702;
        public const int GROUP_LIST = 420;
        public const int GROUP_MEMBER = 265;
        public const int GROUP_MEMBERS = 1200;
        public const int GROUP_MEMBERS_REFRESH = 2445;
        public const int GROUP_MEMBER_REMOVE_CONFIRM = 1876;
        public const int GROUP_PURCHASED = 2808;
        public const int GROUP_SETTINGS = 3965;
        public const int GROUP_BADGE_PARTS = 2238;
        public const int ITEM_DIMMER_SETTINGS = 2710;
        public const int ITEM_STACK_HELPER = 2816;
        public const int ITEM_WALL = 1369;
        public const int ITEM_WALL_ADD = 2187;
        public const int ITEM_WALL_REMOVE = 3208;
        public const int ITEM_WALL_UPDATE = 2009;
        public const int LOAD_GAME_URL = 2624;
        public const int MARKETPLACE_CONFIG = 1823;
        public const int MESSENGER_ACCEPT_FRIENDS = 896;
        public const int MESSENGER_CHAT = 1587;
        public const int MESSENGER_FIND_FRIENDS = 1210;
        public const int MESSENGER_FOLLOW_FAILED = 3048;
        public const int MESSENGER_FRIEND_NOTIFICATION = 3082;
        public const int MESSENGER_FRIENDS = 3130;
        public const int MESSENGER_INIT = 1605;
        public const int MESSENGER_INSTANCE_MESSAGE_ERROR = 3359;
        public const int MESSENGER_INVITE = 3870;
        public const int MESSENGER_INVITE_ERROR = 462;
        public const int MESSENGER_MESSAGE_ERROR = 892;
        public const int MESSENGER_MINIMAIL_COUNT = 2803;
        public const int MESSENGER_MINIMAIL_NEW = 1911;
        public const int MESSENGER_RELATIONSHIPS = 2016;
        public const int MESSENGER_REQUEST = 2219;
        public const int MESSENGER_REQUEST_ERROR = 892;
        public const int MESSENGER_REQUESTS = 280;
        public const int MESSENGER_ROOM_INVITE = 3870;
        public const int MESSENGER_SEARCH = 973;
        public const int MESSENGER_UPDATE = 2800;
        public const int MODERATION_REPORT_DISABLED = 1651;
        public const int MODERATION_TOOL = 2696;
        public const int MODERATION_USER_INFO = 2866;
        public const int MOTD_MESSAGES = 2035;
        public const int NAVIGATOR_CATEGORIES = 1562;
        public const int NAVIGATOR_COLLAPSED = 1543;
        public const int NAVIGATOR_EVENT_CATEGORIES = 3244;
        public const int NAVIGATOR_LIFTED = 3104;
        public const int NAVIGATOR_METADATA = 3052;
        public const int NAVIGATOR_OPEN_ROOM_CREATOR = 2064;
        public const int NAVIGATOR_SEARCH = 2690;
        public const int NAVIGATOR_SEARCHES = 3984;
        public const int NAVIGATOR_SETTINGS = 518;
        public const int NOTIFICATION_LIST = 1992;
        public const int PET_FIGURE_UPDATE = 1924;
        public const int PET_INFO = 2901;
        public const int RECYCLER_PRIZES = 3164;
        public const int ROOM_BAN_LIST = 1869;
        public const int ROOM_BAN_REMOVE = 3429;
        public const int ROOM_CREATED = 1304;
        public const int ROOM_DOORBELL = 2309;
        public const int ROOM_DOORBELL_ACCEPTED = 3783;
        public const int ROOM_DOORBELL_REJECTED = 878;
        public const int ROOM_ENTER = 758;
        public const int ROOM_ENTER_ERROR = 899;
        public const int ROOM_FORWARD = 160;
        public const int ROOM_HEIGHT_MAP = 2753;
        public const int ROOM_HEIGHT_MAP_UPDATE = 558;
        public const int ROOM_INFO = 687;
        public const int ROOM_INFO_OWNER = 749;
        public const int ROOM_MODEL = 1301;
        public const int ROOM_MODEL_BLOCKED_TILES = 3990;
        public const int ROOM_MODEL_DOOR = 1664;
        public const int ROOM_MODEL_NAME = 2031;
        public const int ROOM_MUTED = 2533;
        public const int ROOM_MUTE_USER = 826;
        public const int ROOM_PAINT = 2454;
        public const int ROOM_PROMOTION = 2274;
        public const int ROOM_QUEUE_STATUS = 2208;
        public const int ROOM_RIGHTS = 780;
        public const int ROOM_RIGHTS_CLEAR = 2392;
        public const int ROOM_RIGHTS_LIST = 1284;
        public const int ROOM_RIGHTS_LIST_ADD = 2088;
        public const int ROOM_RIGHTS_LIST_REMOVE = 1327;
        public const int ROOM_RIGHTS_OWNER = 339;
        public const int ROOM_ROLLING = 3207;
        public const int ROOM_SCORE = 482;
        public const int ROOM_SETTINGS = 1498;
        public const int ROOM_SETTINGS_CHAT = 1191;
        public const int ROOM_SETTINGS_SAVE = 948;
        public const int ROOM_SETTINGS_SAVE_ERROR = 1555;
        public const int ROOM_SETTINGS_UPDATED = 3297;
        public const int ROOM_SPECTATOR = 1033;
        public const int ROOM_THICKNESS = 3547;
        public const int ROOM_EFFECT = 4001;
        public const int INFO_FEED_ENABLE = 3284;
        public const int SECURITY_MACHINE = 1488;
        public const int MYSTERY_BOX_KEYS = 2833;
        public const int TRADE_ACCEPTED = 2568;
        public const int TRADE_CLOSED = 1373;
        public const int TRADE_COMPLETED = 1001;
        public const int TRADE_CONFIRMATION = 2720;
        public const int TRADE_LIST_ITEM = 2024;
        public const int TRADE_NOT_OPEN = 3128;
        public const int TRADE_OPEN = 2505;
        public const int TRADE_OPEN_FAILED = 217;
        public const int TRADE_OTHER_NOT_ALLOWED = 2154;
        public const int TRADE_YOU_NOT_ALLOWED = 3058;
        public const int UNIT = 374;
        public const int UNIT_CHANGE_NAME = 2182;
        public const int UNIT_CHAT = 1446;
        public const int UNIT_CHAT_SHOUT = 1036;
        public const int UNIT_CHAT_WHISPER = 2704;
        public const int UNIT_DANCE = 2233;
        public const int UNIT_EFFECT = 1167;
        public const int UNIT_EXPRESSION = 1631;
        public const int UNIT_HAND_ITEM = 1474;
        public const int UNIT_IDLE = 1797;
        public const int UNIT_INFO = 3920;
        public const int UNIT_NUMBER = 2324;
        public const int UNIT_REMOVE = 2661;
        public const int UNIT_STATUS = 1640;
        public const int UNIT_TYPING = 1717;
        public const int UNSEEN_ITEMS = 2103;
        public const int USER_ACHIEVEMENT_SCORE = 1968;
        public const int USER_BADGES = 717;
        public const int USER_BADGES_ADD = 2493;
        public const int USER_BADGES_CURRENT = 1087;
        public const int USER_BOT_REMOVE = 233;
        public const int USER_BOTS = 3086;
        public const int USER_CHANGE_NAME = 118;
        public const int USER_CLOTHING = 1450;
        public const int USER_CREDITS = 3475;
        public const int USER_CURRENCY = 2018;
        public const int ACTIVITY_POINT_NOTIFICATION = 2275;
        public const int USER_EFFECTS = 340;
        public const int USER_FAVORITE_ROOM = 2524;
        public const int USER_FAVORITE_ROOM_COUNT = 151;
        public const int USER_FIGURE = 2429;
        public const int USER_FURNITURE = 994;
        public const int USER_FURNITURE_ADD = 104;
        public const int USER_FURNITURE_POSTIT_PLACED = 1501;
        public const int USER_FURNITURE_REFRESH = 3151;
        public const int USER_FURNITURE_REMOVE = 159;
        public const int USER_HOME_ROOM = 2875;
        public const int USER_IGNORED = 126;
        public const int USER_IGNORED_RESULT = 207;
        public const int USER_INFO = 2725;
        public const int USER_OUTFITS = 3315;
        public const int USER_PERKS = 2586;
        public const int USER_PERMISSIONS = 411;
        public const int USER_PET_ADD = 2101;
        public const int USER_PET_REMOVE = 3253;
        public const int USER_PETS = 3522;
        public const int USER_PROFILE = 3898;
        public const int USER_RESPECT = 2815;
        public const int USER_SANCTION_STATUS = 3679;
        public const int USER_SETTINGS = 513;
        public const int USER_SUBSCRIPTION = 954;
        public const int USER_WARDROBE_PAGE = 3315;
        public const int WIRED_ACTION = 1434;
        public const int WIRED_CONDITION = 1108;
        public const int WIRED_ERROR = 156;
        public const int WIRED_OPEN = 1830;
        public const int WIRED_REWARD = 178;
        public const int WIRED_SAVE = 1155;
        public const int WIRED_TRIGGER = 383;
        public const int PLAYING_GAME = 448;
        public const int FURNITURE_STATE_2 = 3431;
        public const int REMOVE_BOT_FROM_INVENTORY = 233;
        public const int ADD_BOT_TO_INVENTORY = 1352;
        public const int ACHIEVEMENT_PROGRESSED = 2107;
        public const int MODTOOL_ROOM_INFO = 1333;
        public const int MODTOOL_USER_CHATLOG = 3377;
        public const int MODTOOL_ROOM_CHATLOG = 3434;
        public const int MODTOOL_VISITED_ROOMS_USER = 1752;
        public const int MODERATOR_ACTION_RESULT = 2335;
        public const int ISSUE_DELETED = 3192;
        public const int ISSUE_INFO = 3609;
        public const int ISSUE_PICK_FAILED = 3150;
        public const int CFH_CHATLOG = 607;
        public const int MODERATOR_TOOL_PREFERENCES = 1576;
        public const int LOVELOCK_FURNI_START = 3753;
        public const int LOVELOCK_FURNI_FRIEND_COMFIRMED = 382;
        public const int LOVELOCK_FURNI_FINISHED = 770;
        public const int GIFT_RECEIVER_NOT_FOUND = 1517;
        public const int GIFT_OPENED = 56;
        public const int FLOOD_CONTROL = 566;
        public const int REMAINING_MUTE = 826;
        public const int USER_EFFECT_LIST = 340;
        public const int USER_EFFECT_LIST_ADD = 2867;
        public const int USER_EFFECT_LIST_REMOVE = 2228;
        public const int USER_EFFECT_ACTIVATE = 1959;
        public const int CLUB_GIFT_INFO = 619;
        public const int REDEEM_VOUCHER_ERROR = 714;
        public const int REDEEM_VOUCHER_OK = 3336;
        public const int IN_CLIENT_LINK = 2023;
        public const int BOT_COMMAND_CONFIGURATION = 1618;
        public const int HAND_ITEM_RECEIVED = 354;
        public const int PET_PLACING_ERROR = 2913;
        public const int BOT_ERROR = 639;
        public const int MARKETPLACE_SELL_ITEM = 54;
        public const int MARKETPLACE_ITEM_STATS = 725;
        public const int MARKETPLACE_OWN_ITEMS = 3884;
        public const int MARKETPLACE_CANCEL_SALE = 3264;
        public const int MARKETPLACE_ITEM_POSTED = 1359;
        public const int MARKETPLACE_ITEMS_SEARCHED = 680;
        public const int MARKETPLACE_AFTER_ORDER_STATUS = 2032;
        public const int CATALOG_RECEIVE_PET_BREEDS = 3331;
        public const int CATALOG_APPROVE_NAME_RESULT = 1503;
        public const int OBJECTS_DATA_UPDATE = 1453;
        public const int PET_EXPERIENCE = 2156;
        public const int COMMUNITY_GOAL_VOTE_EVENT = 1435;
        public const int PROMO_ARTICLES = 286;
        public const int COMMUNITY_GOAL_EARNED_PRIZES = 3319;
        public const int COMMUNITY_GOAL_PROGRESS = 2525;
        public const int CONCURRENT_USERS_GOAL_PROGRESS = 2737;
        public const int QUEST_DAILY = 1878;
        public const int QUEST_CANCELLED = 3027;
        public const int QUEST_COMPLETED = 949;
        public const int COMMUNITY_GOAL_HALL_OF_FAME = 3005;
        public const int NOOBNESS_LEVEL = 3738;
        public const int CAN_CREATE_ROOM_EVENT = 2599;
        public const int FAVORITE_GROUP_UDPATE = 3403;
        public const int EPIC_POPUP = 3945;
        public const int SEASONAL_QUESTS = 1122;
        public const int QUESTS = 3625;
        public const int QUEST = 230;
        public const int BONUS_RARE_INFO = 1533;
        public const int CRAFTABLE_PRODUCTS = 1000;
        public const int CRAFTING_RECIPE = 2774;
        public const int CRAFTING_RECIPES_AVAILABLE = 2124;
        public const int CRAFTING_RESULT = 618;
        public const int CAMERA_PUBLISH_STATUS = 2057;
        public const int CAMERA_PURCHASE_OK = 2783;
        public const int CAMERA_STORAGE_URL = 3696;
        public const int COMPETITION_STATUS = 133;
        public const int INIT_CAMERA = 3878;
        public const int THUMBNAIL_STATUS = 3595;
        public const int ACHIEVEMENT_NOTIFICATION = 806;
        public const int CLUB_GIFT_NOTIFICATION = 2188;
        public const int INTERSTITIAL_MESSAGE = 1808;
        public const int ROOM_AD_ERROR = 1759;
        public const int AVAILABILITY_TIME = 600;
        public const int HOTEL_CLOSED_AND_OPENS = 3728;
        public const int HOTEL_CLOSES_AND_OPENS_AT = 2771;
        public const int HOTEL_WILL_CLOSE_MINUTES = 1050;
        public const int HOTEL_MAINTENANCE = 1350;
        public const int JUKEBOX_PLAYLIST_FULL = 105;
        public const int JUKEBOX_SONG_DISKS = 34;
        public const int NOW_PLAYING = 469;
        public const int OFFICIAL_SONG_ID = 1381;
        public const int PLAYLIST = 1748;
        public const int PLAYLIST_SONG_ADDED = 1140;
        public const int TRAX_SONG_INFO = 3365;
        public const int USER_SONG_DISKS_INVENTORY = 2602;
        public const int CHECK_USER_NAME = 563;
        public const int CFH_SANCTION = 2782;
        public const int CFH_TOPICS = 325;
        public const int CFH_SANCTION_STATUS = 2221;
        public const int CAMPAIGN_CALENDAR_DATA = 2531;
        public const int CAMPAIGN_CALENDAR_DOOR_OPENED = 2551;
        public const int BUILDERS_CLUB_FURNI_COUNT = 3828;
        public const int BUILDERS_CLUB_SUBSCRIPTION = 1452;
        public const int CATALOG_PAGE_EXPIRATION = 2668;
        public const int CATALOG_EARLIEST_EXPIRY = 2515;
        public const int CLUB_GIFT_SELECTED = 659;
        public const int TARGET_OFFER_NOT_FOUND = 1237;
        public const int TARGET_OFFER = 119;
        public const int DIRECT_SMS_CLUB_BUY = 195;
        public const int ROOM_AD_PURCHASE = 2468;
        public const int NOT_ENOUGH_BALANCE = 3914;
        public const int LIMITED_OFFER_APPEARING_NEXT = 44;
        public const int IS_OFFER_GIFTABLE = 761;
        public const int CLUB_EXTENDED_OFFER = 3964;
        public const int SEASONAL_CALENDAR_OFFER = 1889;
        public const int COMPETITION_ENTRY_SUBMIT = 1177;
        public const int COMPETITION_VOTING_INFO = 3506;
        public const int COMPETITION_TIMING_CODE = 1745;
        public const int COMPETITION_USER_PART_OF = 3841;
        public const int COMPETITION_NO_OWNED_ROOMS = 2064;
        public const int COMPETITION_SECONDS_UNTIL = 3926;
        public const int BADGE_POINT_LIMITS = 2501;
        public const int BADGE_REQUEST_FULFILLED = 2998;
        public const int HELPER_TALENT_TRACK = 3406;
        public const int USER_BANNED = 1683;
        public const int BOT_RECEIVED = 3684;
        public const int PET_LEVEL_NOTIFICATION = 859;
        public const int PET_RECEIVED = 1111;
        public const int MODERATION_CAUTION = 1890;
        public const int YOUTUBE_CONTROL_VIDEO = 1554;
        public const int YOUTUBE_DISPLAY_PLAYLISTS = 1112;
        public const int YOUTUBE_DISPLAY_VIDEO = 1411;
        public const int CFH_DISABLED_NOTIFY = 1651;
        public const int QUESTION = 2665;
        public const int POLL_CONTENTS = 2997;
        public const int POLL_ERROR = 662;
        public const int POLL_OFFER = 3785;
        public const int QUESTION_ANSWERED = 2589;
        public const int QUESTION_FINISHED = 1066;
        public const int CFH_PENDING_CALLS = 1121;
        public const int GUIDE_ON_DUTY_STATUS = 1548;
        public const int GUIDE_SESSION_ATTACHED = 1591;
        public const int GUIDE_SESSION_DETACHED = 138;
        public const int GUIDE_SESSION_ENDED = 1456;
        public const int GUIDE_SESSION_ERROR = 673;
        public const int GUIDE_SESSION_INVITED_TO_GUIDE_ROOM = 219;
        public const int GUIDE_SESSION_MESSAGE = 841;
        public const int GUIDE_SESSION_PARTNER_IS_TYPING = 1016;
        public const int GUIDE_SESSION_REQUESTER_ROOM = 1847;
        public const int GUIDE_SESSION_STARTED = 3209;
        public const int GUIDE_TICKET_CREATION_RESULT = 3285;
        public const int GUIDE_TICKET_RESOLUTION = 2674;
        public const int GUIDE_REPORTING_STATUS = 3463;
        public const int HOTEL_MERGE_NAME_CHANGE = 1663;
        public const int ISSUE_CLOSE_NOTIFICATION = 934;
        public const int QUIZ_DATA = 2927;
        public const int QUIZ_RESULTS = 2772;
        public const int CFH_PENDING_CALLS_DELETED = 77;
        public const int CFH_REPLY = 3796;
        public const int CHAT_REVIEW_SESSION_DETACHED = 30;
        public const int CHAT_REVIEW_SESSION_OFFERED_TO_GUIDE = 735;
        public const int CHAT_REVIEW_SESSION_RESULTS = 3276;
        public const int CHAT_REVIEW_SESSION_STARTED = 143;
        public const int CHAT_REVIEW_SESSION_VOTING_STATUS = 1829;
        public const int SCR_SEND_KICKBACK_INFO = 3277;
        public const int PET_STATUS = 1907;
        public const int GROUP_DEACTIVATE = 3129;
        public const int PET_RESPECTED = 2788;
        public const int PET_SUPPLEMENT = 3441;
    }
}
