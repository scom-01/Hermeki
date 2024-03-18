public interface IExecuteEvent
{
    public ItemEventSet ExcuteEvent(ItemEvent_Type type, StatsItemSO parentItem, Unit unit, Unit enemy, ItemEventSet itemEventSet);
}
