


Namespace AcurSoft.XtraGrid.Views.Grid.Summary

    Public Class SummaryItemTypeHelperEx
        Public Const Custom As Integer = 1 << 100
        Public Const Sum As Integer = 1 << 101
        Public Const Avg As Integer = 1 << 102
        Public Const Top As Integer = 1 << 103
        Public Const Buttom As Integer = 1 << 104
        Public Const Percent As Integer = 1 << 105

        Public Shared Function IsStandard(st As SummaryItemTypeEx2) As Boolean
            Return st.value__ < 100
        End Function

        Public Shared Function IsSumOrAvg(st As SummaryItemTypeEx2) As Boolean
            Return IsSum(st) OrElse IsAvg(st)
        End Function

        Public Shared Function IsMinOrMax(st As SummaryItemTypeEx2) As Boolean
            Return st = SummaryItemTypeEx2.Min OrElse st = SummaryItemTypeEx2.Max
        End Function


        Public Shared Function IsSum(st As SummaryItemTypeEx2) As Boolean
            Return st = SummaryItemTypeEx2.Sum OrElse st.HasBitFlag(Of Integer)(SummaryItemTypeHelperEx.Sum)
        End Function

        Public Shared Function IsAvg(st As SummaryItemTypeEx2) As Boolean
            Return st = SummaryItemTypeEx2.Average OrElse st.HasBitFlag(Of Integer)(SummaryItemTypeHelperEx.Avg)
        End Function
        Public Shared Function IsTop(st As SummaryItemTypeEx2) As Boolean
            Return st.HasBitFlag(Of Integer)(SummaryItemTypeHelperEx.Top)
        End Function
        Public Shared Function IsButtom(st As SummaryItemTypeEx2) As Boolean
            Return st.HasBitFlag(Of Integer)(SummaryItemTypeHelperEx.Buttom)
        End Function
        Public Shared Function IsPercent(st As SummaryItemTypeEx2) As Boolean
            Return st.HasBitFlag(Of Integer)(SummaryItemTypeHelperEx.Percent)
        End Function
        Public Shared Function IsTopButtom(st As SummaryItemTypeEx2) As Boolean
            Return IsTop(st) OrElse IsButtom(st)
        End Function

    End Class
End Namespace
