namespace GamingXRCore.Tooltip
{
    public interface IHoverable
    {
        TooltipModel GetTooltipModel();
        void OnHoverEnter();
        void OnHoverExit();
    }
}