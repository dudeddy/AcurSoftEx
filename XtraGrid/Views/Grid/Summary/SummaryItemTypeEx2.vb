Imports DevExpress.Data


Namespace AcurSoft.XtraGrid.Views.Grid.Summary

    Public Enum SummaryItemTypeEx2
        None = SummaryItemType.None
        Count = SummaryItemType.Count
        Sum = SummaryItemType.Sum
        Average = SummaryItemType.Average
        Min = SummaryItemType.Min
        Max = SummaryItemType.Max
        UniqueValuesCount = SummaryItemTypeHelperEx.Custom Or 1 << 201
        Expression = SummaryItemTypeHelperEx.Custom Or 1 << 202
        Sparkline = SummaryItemTypeHelperEx.Custom Or 1 << 203
        TopXSum = SummaryItemTypeHelperEx.Custom Or SummaryItemTypeHelperEx.Top Or SummaryItemTypeHelperEx.Sum
        TopXAvg = SummaryItemTypeHelperEx.Custom Or SummaryItemTypeHelperEx.Top Or SummaryItemTypeHelperEx.Avg
        BottomXSum = SummaryItemTypeHelperEx.Custom Or SummaryItemTypeHelperEx.Buttom Or SummaryItemTypeHelperEx.Sum
        BottomXAvg = SummaryItemTypeHelperEx.Custom Or SummaryItemTypeHelperEx.Buttom Or SummaryItemTypeHelperEx.Avg

        TopXPercentSum = SummaryItemTypeHelperEx.Custom Or SummaryItemTypeHelperEx.Top Or SummaryItemTypeHelperEx.Percent Or SummaryItemTypeHelperEx.Sum
        TopXPercentAvg = SummaryItemTypeHelperEx.Custom Or SummaryItemTypeHelperEx.Top Or SummaryItemTypeHelperEx.Percent Or SummaryItemTypeHelperEx.Avg
        BottomXPercentSum = SummaryItemTypeHelperEx.Custom Or SummaryItemTypeHelperEx.Buttom Or SummaryItemTypeHelperEx.Percent Or SummaryItemTypeHelperEx.Sum
        BottomXPercentAvg = SummaryItemTypeHelperEx.Custom Or SummaryItemTypeHelperEx.Buttom Or SummaryItemTypeHelperEx.Percent Or SummaryItemTypeHelperEx.Avg

    End Enum
End Namespace
