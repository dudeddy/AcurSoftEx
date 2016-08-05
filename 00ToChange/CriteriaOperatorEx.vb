Imports AcurSoft.Data.Filtering.Helpers
Imports DevExpress.Data.Filtering
Imports DevExpress.Data.Filtering.Helpers
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Filtering

Namespace AcurSoft.Data.Filtering


    Public Class CriteriaOperatorEx
        Inherits CriteriaOperator

        Private _FirstOperand As CriteriaOperator
        Private _ClauseType As CustomClauseType

        Public Sub New(ByVal firstOperand As CriteriaOperator, ByVal clauseType As CustomClauseType)
            _FirstOperand = firstOperand
            _ClauseType = clauseType
        End Sub


        Public Overrides Sub Accept(ByVal visitor As ICriteriaVisitor)
            Me.Accept(visitor.GetType())
        End Sub

        Protected Overrides Function CloneCommon() As CriteriaOperator
            Throw New NotImplementedException()
            'Return MyBase.CloneCommon()
        End Function

        Public Overloads Function Accept(type As Type) As Object
            If type Is GetType(CriteriaToTreeProcessor) Then
                Dim filterControl As New FilterControl
                Dim winFilterTreeNodeModel As New WinFilterTreeNodeModel(filterControl)
                Dim filterControlNodesFactory As New FilterControlNodesFactory(winFilterTreeNodeModel)
                Return CriteriaToTreeProcessor.GetTree(filterControlNodesFactory, _FirstOperand, Nothing)
            ElseIf type Is GetType(CriteriaToStringVisitResult) Then
                Dim filterString As String = String.Format("{1}({0})", _FirstOperand.LegacyToString().Replace("'", ""), _ClauseType)
                Return New CriteriaToStringVisitResult(filterString)
            ElseIf type Is GetType(MsSqlWhereGenerator) Then
                Throw New NotImplementedException()

                'Select Case _ClauseType
                '    Case CustomClauseType.Last30Days
                '        Return CriteriaToWhereClauseHelper.GetMsSqlWhere(New BetweenOperator(_FirstOperand, Date.Now.Date, Date.Now.Date.AddDays(-30)))
                '    Case CustomClauseType.Last90Days
                '        Return CriteriaToWhereClauseHelper.GetMsSqlWhere(New BetweenOperator(_FirstOperand, Date.Now.Date, Date.Now.Date.AddDays(-90)))
                '    Case CustomClauseType.InTheNext90Days
                '        Return CriteriaToWhereClauseHelper.GetMsSqlWhere(New BetweenOperator(_FirstOperand, Date.Now.Date, Date.Now.Date.AddDays(30)))
                '    Case CustomClauseType.InTheNext30Days
                '        Return CriteriaToWhereClauseHelper.GetMsSqlWhere(New BetweenOperator(_FirstOperand, Date.Now.Date, Date.Now.Date.AddDays(90)))
                '    Case CustomClauseType.BeforeToday
                '        Return CriteriaToWhereClauseHelper.GetMsSqlWhere(New BinaryOperator(_FirstOperand, Date.Now.Date, BinaryOperatorType.Less))
                '    Case CustomClauseType.BeforeOrOnToday
                '        Return CriteriaToWhereClauseHelper.GetMsSqlWhere(New BinaryOperator(_FirstOperand, Date.Now.Date, BinaryOperatorType.LessOrEqual))
                '    Case CustomClauseType.AfterToday
                '        Return CriteriaToWhereClauseHelper.GetMsSqlWhere(New BinaryOperator(_FirstOperand, Date.Now.Date, BinaryOperatorType.Greater))
                '    Case CustomClauseType.AfterOrOnToday
                '        Return CriteriaToWhereClauseHelper.GetMsSqlWhere(New BinaryOperator(_FirstOperand, Date.Now.Date, BinaryOperatorType.GreaterOrEqual))
                '    Case Else
                '        Return String.Empty
                'End Select

            End If
            Return Nothing
        End Function


        Public Overrides Function Accept(Of T)(ByVal visitor As ICriteriaVisitor(Of T)) As T
            Return DirectCast(Me.Accept(GetType(T)), T)
        End Function
    End Class

End Namespace
