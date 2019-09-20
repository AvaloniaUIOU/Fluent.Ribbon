﻿namespace Fluent.Automation.Peers
{
    using System.Windows;
    using System.Windows.Automation.Peers;
    using System.Windows.Automation.Provider;
    using JetBrains.Annotations;

    /// <summary>
    /// Automation peer for <see cref="RibbonTabControl"/>.
    /// </summary>
    public class RibbonTabControlAutomationPeer : SelectorAutomationPeer, ISelectionProvider
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public RibbonTabControlAutomationPeer([NotNull] RibbonTabControl owner)
            : base(owner)
        {
            this.OwningRibbonTabControl = owner;
        }

        private RibbonTabControl OwningRibbonTabControl { get; }

        /// <inheritdoc />
        protected override ItemAutomationPeer CreateItemAutomationPeer(object item)
        {
            return new RibbonTabItemDataAutomationPeer(item, this);
        }

        /// <inheritdoc />
        protected override string GetClassNameCore()
        {
            return "RibbonTabControl";
        }

        /// <inheritdoc />
        protected override Point GetClickablePointCore()
        {
            return new Point(double.NaN, double.NaN);
        }

        bool ISelectionProvider.IsSelectionRequired => true;

        bool ISelectionProvider.CanSelectMultiple => false;

        /// <inheritdoc />
        public override object GetPattern(PatternInterface patternInterface)
        {
            switch (patternInterface)
            {
                case PatternInterface.Scroll:
                    var ribbonTabsContainerPanel = this.OwningRibbonTabControl.TabsContainer;
                    if (ribbonTabsContainerPanel != null)
                    {
                        var automationPeer = CreatePeerForElement(ribbonTabsContainerPanel);
                        if (automationPeer != null)
                        {
                            return automationPeer.GetPattern(patternInterface);
                        }
                    }

                    var ribbonTabsContainer = this.OwningRibbonTabControl.TabsContainer as RibbonTabsContainer;
                    if (ribbonTabsContainer != null
                        && ribbonTabsContainer.ScrollOwner != null)
                    {
                        var automationPeer = CreatePeerForElement(ribbonTabsContainer.ScrollOwner);
                        if (automationPeer != null)
                        {
                            automationPeer.EventsSource = this;
                            return automationPeer.GetPattern(patternInterface);
                        }
                    }

                    break;
            }

            return base.GetPattern(patternInterface);
        }
    }
}