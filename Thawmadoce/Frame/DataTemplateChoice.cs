using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Thawmadoce.Frame.Extensions;

namespace Thawmadoce.Frame
{
    [ContentProperty("Templates")]
    public class DataTemplateChoice : DataTemplateSelector
    {
        public DataTemplateChoice()
        {
            Templates = new DataTemplates();
        }

        public DataTemplates Templates { get; private set; }

        public event EventHandler<TemplateProvisionEventArgs> BeforeTemplateProvision;

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var dt = Templates.GetMatchFor(item.GetType()) ?? base.SelectTemplate(item, container);
            BeforeTemplateProvision.Raise(this, new TemplateProvisionEventArgs(item, dt));
            return dt;
        }
    }

    public class DataTemplates : List<DataTemplate>
    {
        internal DataTemplate GetMatchFor(Type objectType)
        {
            var dataTemplate = this.FirstOrDefault(t => MatchViaDataType(t, objectType));
            return dataTemplate;
        }

        private static bool MatchViaDataType(DataTemplate arg, Type objectType)
        {
            var type = arg.DataType as Type;
            return type != null && type.IsAssignableFrom(objectType);
        }
    }

    public class TemplateProvisionEventArgs : EventArgs
    {
        public object DataContext { get; private set; }
        public DataTemplate DataTemplate { get; private set; }

        public TemplateProvisionEventArgs(object dataContext, DataTemplate dataTemplate)
        {
            DataContext = dataContext;
            DataTemplate = dataTemplate;
        }
    }
}