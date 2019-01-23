///* Empiria Steps *********************************************************************************************
//*                                                                                                            *
//*  Solution : Empiria Steps                                    System  : Steps Domain Models                 *
//*  Assembly : Empiria.ProjectManagement.dll                    Pattern : Composite Component                 *
//*  Type     : ProjectModel                                     License : Please read LICENSE.txt file        *
//*                                                                                                            *
//*  Summary  : Represents a tree composition of resources applied to projects.                                *
//*                                                                                                            *
//************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
//using System;
//using System.Collections.Generic;

//using Empiria.Contacts;

//namespace Empiria.ProjectManagement.Resources {

//  //public abstract class Component {


//  //}

//  //public class Composite : Component {

//  //  public FixedList<Component> Children {
//  //    get;
//  //  }

//  //}


//  //public class Leaf : Component {


//  //}

//  //public class Folder : TreeItem {

//  //}

//  public class TreeItem {

//    private List<TreeItem> children = new List<TreeItem>();

//    private object target = null;

//    internal TreeItem(List<TreeItem> children) {
//       this.children = children;
//    }

//    internal TreeItem(Resource resource) {
//      this.target = resource;
//      this.Name = resource.Name;
//    }

//    //internal TreeItem(Folder folder) {
//    //  this.target = folder;
//    //}

//    public int Id {
//      get;
//    }

//    public string Name {
//      get;
//    }

//    public FixedList<TreeItem> Children {
//      get {
//        return this.children.ToFixedList();
//      }
//    }

//    public TreeItem Parent {
//      get;
//    }

//    public int Position {
//      get;
//      private set;
//    }

//    public T GetTarget<T>() {
//      return (T) this.target;
//    }

//    public TreeItem Add(Resource resource, int position = -1) {
//      var treeItem = new TreeItem(resource);

//      if (position == -1) {
//        position = this.children.Count;
//      } else {
//        var c = Children.FindAll((x) => x.Position >= position);

//        foreach (var item in c) {
//          item.Position += item.Position + 1;
//        }

//      }
//      children.Insert(position, treeItem);

//      return treeItem;
//    }

//    //public TreeItem Add(Folder folder, int position = -1) {
//    //  var treeItem = new TreeItem(folder);

//    //  if (position == -1) {
//    //    position = this.children.Count;
//    //  } else {
//    //    var c = Children.FindAll((x) => x.Position >= position);

//    //    foreach (var item in c) {
//    //      item.Position += item.Position + 1;
//    //    }

//    //  }
//    //  children.Insert(position, treeItem);

//    //  return treeItem;
//    //}

//  }

//  /// <summary>Represents a tree composition of resources applied to projects.</summary>
//  public class ResourceTree : TreeItem {

//    #region Fields

//    private FixedList<TreeItem> treeItems = new FixedList<TreeItem>();

//    #endregion Fields

//    #region Constructors and parsers

//    private ResourceTree(List<TreeItem> treeItems) : base(treeItems) {
//      Assertion.AssertObject(treeItems, "treeItems");

//      this.treeItems = treeItems;
//      this.Load();
//    }

//    static public ResourceTree Parse() {
//      var owner = Contact.Parse(51);

//      var treeItems = Resource.GetTreeItems(owner);

//      return new ResourceTree(treeItems);
//    }

//    #endregion Constructors and parsers

//    #region Public properties

//    public FixedList<TreeItem> Items {
//      get;
//    }


//    #endregion Public properties

//    #region Methods

//    private void Load() {
//      var roots = treeItems.FindAll((x) => x.Parent.Id == -1);

//      foreach (var item in roots) {

//      }
//    }

//    #endregion Methods

//  } // class ResourceTree

//} // namespace Empiria.ProjectManagement.Resources
