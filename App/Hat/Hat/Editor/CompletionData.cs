using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Hat.Editor
{
    public class CompletionData : ICompletionData
    {
        /*
        public string Text { get; private set; }

        public CompletionData(string text)
        {
            this.Text = text;
        }

        ImageSource ICompletionData.Image
        {
            get { return null; }
        }

        string ICompletionData.Text
        {
            get { return this.Text; } 
        }

        object ICompletionData.Content
        {
            get { return this.Text; }

        }

        object ICompletionData.Description
        {
            get { return this.Text; }
        }

        double ICompletionData.Priority
        { 
            get { return 0; } 
        }

        void ICompletionData.Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
            textArea.Document.Replace(completionSegment, this.Text);
        }
        */

        public CompletionData(string text, string description)
        {
            this.Text = text;
            this.TextDescription = description;
        }

        public System.Windows.Media.ImageSource Image
        {
            get { return null; }
        }

        public string Text { get; private set; }
        public string TextDescription { get; private set; }

        public object Content
        {
            get { return this.Text; }
        }

        public object Description
        {
            get { return this.TextDescription; }
        }

        public void Complete(TextArea textArea, ISegment completionSegment,
            EventArgs insertionRequestEventArgs)
        {
            textArea.Document.Replace(completionSegment, this.Text);
        }

        double ICompletionData.Priority
        {
            get { return 0; }
        }
    }
}
