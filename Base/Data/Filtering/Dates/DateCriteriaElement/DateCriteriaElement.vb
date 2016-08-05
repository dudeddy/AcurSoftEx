
Imports DevExpress.Data.Filtering


Namespace AcurSoft.Data.Filtering
    Public Class DateCriteriaElement
        Inherits DateCriteriaElement(Of Integer)
        Private _Criteria As CriteriaOperator
        Public Overrides Property Criteria As CriteriaOperator
            Get
                If _Criteria Is Nothing Then
                    Select Case DateChooseKind
                        Case DateChooseKind.Choose
                            _Criteria = New OperandValue(Me.Value)
                        Case DateChooseKind.This, DateChooseKind.After, DateChooseKind.Ago
                            Me.GetAfterAgoValueCriteria()
                    End Select
                End If
                Return _Criteria
            End Get
            Set(value As CriteriaOperator)
                _Criteria = value
            End Set
        End Property

        Public Sub New(v As Integer)
            MyBase.New(v, 0)
        End Sub

        Public Sub New(v As Integer, thisValue As Integer)
            MyBase.New(v, thisValue)
        End Sub

        Public Overrides Function GetAfterAgoValueCriteria() As CriteriaOperator
            If Me.DateChooseKind <> DateChooseKind.Choose Then
                Dim sufix As String = ""
                Select Case Me.DateChooseKind
                    Case DateChooseKind.Ago
                        sufix = " - " & Me.AfterAgoValue
                    Case DateChooseKind.After
                        sufix = " + " & Me.AfterAgoValue
                End Select

                _Criteria = CriteriaOperator.Parse(Me.ThisCriteria & sufix)
                Me.GetValueFromCriteria()
            End If
            Return _Criteria
        End Function
    End Class


End Namespace
