using System;

namespace Peasouper.Domain
{
    public class Event : EventMiniView
    {
        public EventType Type { get; set; }
        public string ContentText { get; set; }
        public string ContentHtml { get; set; }
        public Attachment[] Attachments { get; set; }

        public EmailSummary EmailSummaryFull { get; set; }

        public override EmailSummaryMiniView EmailSummary
        {
            get { return EmailSummaryFull; }
            set
            {
                base.EmailSummary = value;
            }
        }
    }

    public class EventMiniView
    {
        public EventId Id { get; set; }
        /// <summary>
        /// sVerb
        /// </summary>
        public string DescriptionEnglish { get; set; }
        public string Description { get; set; }
        public string ChangesSummary { get; set; }

        /// <summary>
        /// TODO: Person's email is not provided, only Id + name
        /// </summary>
        public Person Person { get; set; }
        public PersonId AssignedTo { get; set; }
        public DateTime Date { get; set; }
        public bool IsEmail { get; set; }
        public bool IsExternal { get; set; }
        public EventFormat Format { get; set; }

        public virtual EmailSummaryMiniView EmailSummary { get; set; }
    }

    public class EmailSummaryMiniView
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string ReplyTo { get; set; }
        public string Subject { get; set; }
        public DateTime Date { get; set; }
    }

    public class EmailSummary : EmailSummaryMiniView
    {
        public string BodyText { get; set; }
        public string BodyHtml { get; set; }
    }

    public class Attachment
    {
        public string Filename { get; set; }
        public Uri Url { get; set; }
    }

    public enum EventFormat
    {
        PlainText,
        Html
    }

    public enum EventType
    {
    }
}
