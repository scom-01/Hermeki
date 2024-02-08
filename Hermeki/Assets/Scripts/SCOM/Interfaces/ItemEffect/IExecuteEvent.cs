public interface IExecuteEvent
{
    public ItemEventSet ExcuteEvent(ITEM_TPYE type, StatsItemSO parentItem, Unit unit, Unit enemy, ItemEventSet itemEventSet);
}
