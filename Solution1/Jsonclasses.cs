using System;
using System.Collections.Generic;

namespace JsonTypes {
    //getMe
    class getMe {
        public bool ok { get; set; }
        public User result { get; set; }
    }
    //getUpdates
    public class getUpdates {
        public bool ok { get; set; }
        public List<Update> result { get; set; }
    }

    //STUFF
    public class Update {
        public int update_id { get; set; }
        public Message message { get; set; }
        public Message edited_message { get; set; }
        /*//the floowing are for inline mode and their classes are not implemented yet.
        public InlineQuery inline_query { get; set; }
        public ChosenInlineResult chosen_inline_result { get; set; }
        public CallbackQuery callback_query { get; set; }
        */
    }
    public class User {
        public int id { get; set; }
        public string first_name { get; set; }
        public string username { get; set; }
    }
    public class Chat {
        public int id { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public string username { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
    }
    public class Message {
        public int message_id { get; set; }
        public User from { get; set; }
        public int date { get; set; }
        public Chat chat { get; set; }
        public User forward_from { get; set; }
        public Chat forward_from_chat { get; set; }
        public int forward_date { get; set; }
        public Message reply_to_message { get; set; }
        public int edit_date { get; set; }
        public string text { get; set; }
        public List<MessageEntity> entities { get; set; }
        public Audio audio { get; set; }
        public Document document { get; set; }
        public List<PhotoSize> photo { get; set; }
        public Sticker sticker { get; set; }
        public Video video { get; set; }
        public Voice voice { get; set; }
        public string caption { get; set; }
        public Contact contact { get; set; }
        public Location location { get; set; }
        public Venue venue { get; set; }
        public User new_chat_member { get; set; }
        public User left_chat_member { get; set; }
        public string new_chat_title { get; set; }
        public List<PhotoSize> new_chat_photo { get; set; }
        public bool delete_chat_photo { get; set; }
        public bool group_chat_created { get; set; }
        public bool supergroup_chat_created { get; set; }
        public bool channel_chat_created { get; set; }
        public Int64 migrate_to_chat_id { get; set; }
        public Int64 migrate_from_chat_id { get; set; }
        public Message pinned_message { get; set; }
    }
    public class MessageEntity {
        public string type { get; set; }
        public int offset { get; set; }
        public int length { get; set; }
        public string url { get; set; }
        public User user { get; set; }
    }
    public class PhotoSize {
        public string file_id { get; set; }
        public int widht { get; set; }
        public int height { get; set; }
        public int file_size { get; set; }
    }
    public class Audio {
        public string file_id { get; set; }
        public int duration { get; set; }
        public string performer { get; set; }
        public string title { get; set; }
        public string mime_type { get; set; }
        public string file_size { get; set; }
    }
    public class Document {
        public string file_id { get; set; }
        public PhotoSize thumb { get; set; }
        public string file_name { get; set; }
        public string mime_type { get; set; }
        public int file_size { get; set; }
    }
    public class Sticker {
        public string file_id { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public PhotoSize thumb { get; set; }
        public string emoji { get; set; }
        public int file_size { get; set; }
    }
    public class Video {
        public string file_id { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int duration { get; set; }
        public PhotoSize thumb { get; set; }
        public string mime_type { get; set; }
        public int file_size { get; set; }
    }
    public class Voice {
        public string file_id { get; set; }
        public int duration { get; set; }
        public string mime_type { get; set; }
        public int file_size { get; set; }
    }
    public class Contact {
        public string phone_number { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public int user_id { get; set; }
    }
    public class Location {
        public float longitude { get; set; }
        public float latitude { get; set; }
    }
    public class Venue {
        public Location location { get; set; }
        public string title { get; set; }
        public string address { get; set; }
        public string foursquare_id { get; set; }
    }
    public class UserProfilePhotos {
        public int total_count { get; set; }
        public List<List<PhotoSize>> photos { get; set; }
    }
}
