Imports DevExpress.Data

Imports DevExpress.XtraGrid


Namespace AcurSoft.XtraGrid.Views.Grid.Extenders

    Public Class ColumnSummaryTagInfoyyy
        Public Property SummaryType As SummaryItemTypeEx2
        Public Property SummaryItem As GridSummaryItem
        Private _Info As Object
        Public Property Info As Object
            Get
                Return CustomSummaryHelper.FixSummaryInfo(Me.SummaryType, _Info)
            End Get
            Set(value As Object)
                _Info = value
            End Set
        End Property

        'Public Shared Function GetInstance(si As GridSummaryItem) As ColumnSummaryTagInfo
        '    Return New ColumnSummaryTagInfo(si)
        'End Function

        Public Sub New(si As GridSummaryItem)
            Me.SummaryItem = si
            If si.SummaryType <> SummaryItemType.Custom AndAlso System.Enum.IsDefined(GetType(SummaryItemType), si.SummaryType) Then
                Me.SummaryType = si.SummaryType.AsOf(Of SummaryItemTypeEx2)
            ElseIf si.SummaryType = SummaryItemType.Custom AndAlso si.Tag Is Nothing
                Me.SummaryType = SummaryItemTypeEx2.None
            ElseIf si.SummaryType = SummaryItemType.Custom AndAlso TypeOf si.Tag Is ColumnSummaryTagInfo
                With DirectCast(si.Tag, ColumnSummaryTagInfo)
                    Me.SummaryType = .SummaryType
                    Me.Info = .Info
                End With
            End If
            si.Tag = Me
        End Sub
        Public Sub New(si As GridSummaryItem, summaryType As SummaryItemTypeEx2, Optional info As Object = Nothing)
            If summaryType.IsStandard() Then
                si.SummaryType = si.SummaryType.AsOf(Of SummaryItemType)
            Else
                si.SummaryType = SummaryItemType.Custom
            End If


            Me.SummaryItem = si

            Me.SummaryType = summaryType
            Me.Info = info
            si.Tag = Me
        End Sub

        'Public Sub FixInfo()
        '    If Me.Info Is Nothing Then
        '        If CustomSummaryHelper.IsSummaryTypePercentX(Me.SummaryType) Then
        '            Me.Info = 50
        '        ElseIf CustomSummaryHelper.IsSummaryTypeTopBottomX(Me.SummaryType)
        '            Me.Info = 50
        '        End If
        '    End If
        'End Sub

        Public Sub SetSummaryItem(Optional displayFormat As String = Nothing)
            Me.SummaryItem.Tag = Me
            If Me.IsStandard Then
                Me.SummaryItem.SummaryType = Me.SummaryType.AsOf(Of SummaryItemType)
            Else
                Me.SummaryItem.SummaryType = SummaryItemType.Custom
            End If
            Select Case Me.SummaryType
                Case SummaryItemTypeEx2.TopXSum
                    If Me.Info Is Nothing Then
                        Me.Info = 5
                    End If
            End Select
            If displayFormat IsNot Nothing Then
                Me.SummaryItem.DisplayFormat = displayFormat
            End If
        End Sub

        'Public Function IsStandard() As Boolean
        '    If Me.SummaryItem.SummaryType = SummaryItemType.Custom AndAlso Me.SummaryItem.Tag IsNot Nothing AndAlso TypeOf Me.SummaryItem.Tag Is ColumnSummaryTagInfo Then
        '        Return True
        '    End If
        '    Return False
        'End Function
        Public Function IsStandard() As Boolean
            Return Me.SummaryType.value__ < 100
        End Function

    End Class
End Namespace
