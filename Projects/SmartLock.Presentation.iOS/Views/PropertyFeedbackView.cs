using System;
using System.Collections.Generic;
using SmartLock.Model.Models;
using SmartLock.Presentation.Core.ViewControllers;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.iOS.Controls.Sources;
using SmartLock.Presentation.iOS.Support;
using SmartLock.Presentation.iOS.Views.ViewBases;
using UIKit;

namespace SmartLock.Presentation.iOS.Views
{
    public partial class PropertyFeedbackView : View<IPropertyFeedbackView>, IPropertyFeedbackView
    {
        private PropertyFeedbackSource _propertyFeedbackSource;

        public event Action BackClick;
        public event Action<string> SubmitClick;

        public PropertyFeedbackView(PropertyFeedbackController controller) : base(controller, "PropertyFeedbackView")
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ConfigureUI();

            IvBack.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                BackClick?.Invoke();
            }));

            BtnSubmit.TouchUpInside += (s, e) => SubmitClick?.Invoke(EtFeedback.Text);
        }

        public void Show()
        {
            UpdateUI(false);
        }

        public void Show(List<PropertyFeedback> propertyFeedbacks)
        {
            UpdateUI(true);

            if (_propertyFeedbackSource == null)
            {
                _propertyFeedbackSource = new PropertyFeedbackSource(propertyFeedbacks);

                FeedbackTableView.EstimatedRowHeight = 246f;
                FeedbackTableView.RowHeight = UITableView.AutomaticDimension;
                FeedbackTableView.Source = _propertyFeedbackSource;
            }
            else
            {
                _propertyFeedbackSource.PropertyFeedbacks = propertyFeedbacks;
            }

            FeedbackTableView.ReloadData();
        }

        private void ConfigureUI()
        {
            ShadowHelper.AddShadow(EtFeedback);
        }

        private void UpdateUI(bool showTableView)
        {
            EtFeedback.Hidden = showTableView;
            BtnSubmit.Hidden = showTableView;
            FeedbackTableView.Hidden = !showTableView;
        }
    }
}

