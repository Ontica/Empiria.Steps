/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Legal Domain                  *
*  Assembly : Empiria.Steps.dll                                Pattern : Power type                          *
*  Type     : LegalDocumentType                                License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Power type that defines a legal document type such as a legislation, regulation or a contract. *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Ontology;

namespace Empiria.Steps.Legal {

  /// <summary>Power type that defines a legal document type such as a legislation,
  /// a regulation or a contract.</summary>
  [Powertype(typeof(Contract))]
  public sealed class LegalDocumentType : Powertype {

    #region Constructors and parsers

    private LegalDocumentType() {
      // Empiria power types always have this constructor.
    }


    public static LegalDocumentType Contract {
      get {
        return ObjectTypeInfo.Parse<LegalDocumentType>("ObjectType.LegalDocument.Contract");
      }
    }


    public static LegalDocumentType Regulation {
      get {
        return ObjectTypeInfo.Parse<LegalDocumentType>("ObjectType.LegalDocument.Regulation");
      }
    }


    static public new LegalDocumentType Parse(int typeId) {
      return ObjectTypeInfo.Parse<LegalDocumentType>(typeId);
    }


    static internal new LegalDocumentType Parse(string typeName) {
      return ObjectTypeInfo.Parse<LegalDocumentType>(typeName);
    }

    #endregion Constructors and parsers

    #region Public methods

    /// <summary>Factory method to create LegalDocument instances of this legal document type.</summary>
    internal Contract CreateInstance() {
      return base.CreateObject<Contract>();
    }

    #endregion Public methods

  } // class LegalDocumentType

} // namespace Empiria.Steps.Legal
