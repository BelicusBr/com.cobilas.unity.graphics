namespace Cobilas.Unity.Graphics.IGU.Interfaces {
    public interface IIGUToolTip {
        string ToolTip { get; set; }
        bool UseTooltip { get; set; }
        IGUStyle TooltipStyle { get; set; }
        void InternalDrawToolTip();
    }
}