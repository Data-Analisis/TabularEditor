
// Code generated by a template
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using TabularEditor.PropertyGridUI;
using TabularEditor.UndoFramework;
using System.Drawing.Design;
using TOM = Microsoft.AnalysisServices.Tabular;
namespace TabularEditor.TOMWrapper
{
  
	/// <summary>
///             Defines a logical view over the Model and is a child of a Model object. It allows hiding Tables, Columns, Measures, and Hierarchies so that end users can look at a smaller subset of the large data model. 
///             </summary>
	[TypeConverter(typeof(DynamicPropertyConverter))]
	public partial class Perspective: TabularNamedObject
			, IDescriptionObject
			, IAnnotationObject
			, ITranslatableObject
			, IClonableObject
	{
	    protected internal new TOM.Perspective MetadataObject { get { return base.MetadataObject as TOM.Perspective; } internal set { base.MetadataObject = value; } }

        [Browsable(true),NoMultiselect,Category("Translations and Perspectives"),Description("The collection of Annotations on this object."),Editor(typeof(AnnotationCollectionEditor), typeof(UITypeEditor))]
		public AnnotationCollection Annotations { get; private set; }
		public string GetAnnotation(int index) {
			return MetadataObject.Annotations[index].Value;
		}
		public string GetAnnotation(string name) {
		    return MetadataObject.Annotations.ContainsName(name) ? MetadataObject.Annotations[name].Value : null;
		}
		public void SetAnnotation(int index, string value, bool undoable = true) {
			var name = MetadataObject.Annotations[index].Name;
			SetAnnotation(name, value, undoable);
		}
		public string GetNewAnnotationName() {
			return MetadataObject.Annotations.GetNewName("New Annotation");
		}
		public void SetAnnotation(string name, string value, bool undoable = true) {
			if(name == null) name = GetNewAnnotationName();

			if(value == null) {
				// Remove annotation if set to null:
				RemoveAnnotation(name, undoable);
				return;
			}

			if(GetAnnotation(name) == value) return;
			bool undoable2 = true;
			bool cancel = false;
			OnPropertyChanging(Properties.ANNOTATIONS, name + ":" + value, ref undoable2, ref cancel);
			if (cancel) return;

			if(MetadataObject.Annotations.Contains(name)) {
				// Change existing annotation:
				var oldValue = GetAnnotation(name);
				MetadataObject.Annotations[name].Value = value;
				if (undoable) Handler.UndoManager.Add(new UndoAnnotationAction(this, name, value, oldValue));
				OnPropertyChanged(Properties.ANNOTATIONS, name + ":" + oldValue, name + ":" + value);
			} else {
				// Add new annotation:
				MetadataObject.Annotations.Add(new TOM.Annotation{ Name = name, Value = value });
				if (undoable) Handler.UndoManager.Add(new UndoAnnotationAction(this, name, value, null));
				OnPropertyChanged(Properties.ANNOTATIONS, null, name + ":" + value);
			}

		}
		public void RemoveAnnotation(string name, bool undoable = true) {
			if(MetadataObject.Annotations.Contains(name)) {
				// Get current value:
				bool undoable2 = true;
				bool cancel = false;
				OnPropertyChanging(Properties.ANNOTATIONS, name + ":" + GetAnnotation(name), ref undoable2, ref cancel);
				if (cancel) return;

				var oldValue = MetadataObject.Annotations[name].Value;
				MetadataObject.Annotations.Remove(name);

				// Undo-handling:
				if (undoable) Handler.UndoManager.Add(new UndoAnnotationAction(this, name, null, oldValue));
				OnPropertyChanged(Properties.ANNOTATIONS, name + ":" + oldValue, null);
			}
		}
		public int GetAnnotationsCount() {
			return MetadataObject.Annotations.Count;
		}
		public IEnumerable<string> GetAnnotations() {
			return MetadataObject.Annotations.Select(a => a.Name);
		}

		/// <summary>
///             Gets or sets the description of the object.
///             </summary><returns>The string contining the description of the object.</returns>
		[DisplayName("Description")]
		[Category("Basic"),Description(@"Gets or sets the description of the object."),IntelliSense("The Description of this Perspective.")][Editor(typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(System.Drawing.Design.UITypeEditor))]
		public string Description {
			get {
			    return MetadataObject.Description;
			}
			set {
				var oldValue = Description;
				if (oldValue == value) return;
				bool undoable = true;
				bool cancel = false;
				OnPropertyChanging(Properties.DESCRIPTION, value, ref undoable, ref cancel);
				if (cancel) return;
				MetadataObject.Description = value;
				if(undoable) Handler.UndoManager.Add(new UndoPropertyChangedAction(this, Properties.DESCRIPTION, oldValue, value));
				OnPropertyChanged(Properties.DESCRIPTION, oldValue, value);
			}
		}
		private bool ShouldSerializeDescription() { return false; }

        /// <summary>
        /// Collection of localized descriptions for this Perspective.
        /// </summary>
        [Browsable(true),DisplayName("Translated Descriptions"),Description("Shows all translated descriptions of this object."),Category("Translations and Perspectives")]
	    public TranslationIndexer TranslatedDescriptions { private set; get; }
        /// <summary>
        /// Collection of localized names for this Perspective.
        /// </summary>
        [Browsable(true),DisplayName("Translated Names"),Description("Shows all translated names of this object."),Category("Translations and Perspectives")]
	    public TranslationIndexer TranslatedNames { private set; get; }


		public static Perspective CreateFromMetadata(TOM.Perspective metadataObject, bool init = true) {
			var obj = new Perspective(metadataObject, init);
			if(init) 
			{
				obj.InternalInit();
				obj.Init();
			}
			return obj;
		}


		/// <summary>
		/// Creates a new Perspective and adds it to the parent Model.
		/// Also creates the underlying metadataobject and adds it to the TOM tree.
		/// </summary>
		public static Perspective CreateNew(Model parent, string name = null)
		{
			var metadataObject = new TOM.Perspective();
			metadataObject.Name = parent.Perspectives.GetNewName(string.IsNullOrWhiteSpace(name) ? "New Perspective" : name);

			var obj = new Perspective(metadataObject);

			parent.Perspectives.Add(obj);
			
			obj.Init();

			return obj;
		}

		/// <summary>
		/// Creates a new Perspective and adds it to the current Model.
		/// Also creates the underlying metadataobject and adds it to the TOM tree.
		/// </summary>		
		public static Perspective CreateNew(string name = null)
		{
			var metadataObject = new TOM.Perspective();
			metadataObject.Name = TabularModelHandler.Singleton.Model.Perspectives.GetNewName(string.IsNullOrWhiteSpace(name) ? "New Perspective" : name);

			var obj = new Perspective(metadataObject);

			TabularModelHandler.Singleton.Model.Perspectives.Add(obj);

			obj.Init();

			return obj;
		}


		/// <summary>
		/// Creates an exact copy of this Perspective object.
		/// </summary>
		public Perspective Clone(string newName = null, bool includeTranslations = true) {
		    Handler.BeginUpdate("Clone Perspective");

			// Create a clone of the underlying metadataobject:
			var tom = MetadataObject.Clone() as TOM.Perspective;


			// Assign a new, unique name:
			tom.Name = Parent.Perspectives.MetadataObjectCollection.GetNewName(string.IsNullOrEmpty(newName) ? tom.Name + " copy" : newName);
				
			// Create the TOM Wrapper object, representing the metadataobject (but don't init until after we add it to the parent):
			var obj = CreateFromMetadata(tom, false);

			// Add the object to the parent collection:
			Parent.Perspectives.Add(obj);

			obj.InternalInit();
			obj.Init();

			// Copy translations, if applicable:
			if(includeTranslations) {
				// TODO: Copy translations of child objects

				obj.TranslatedNames.CopyFrom(TranslatedNames);
				obj.TranslatedDescriptions.CopyFrom(TranslatedDescriptions);
			}

            Handler.EndUpdate();

            return obj;
		}

		TabularNamedObject IClonableObject.Clone(string newName, bool includeTranslations, TabularNamedObject newParent) 
		{
			if (newParent != null) throw new ArgumentException("This object can not be cloned to another parent. Argument newParent should be left as null.", "newParent");
			return Clone(newName, includeTranslations);
		}

	
        internal override void RenewMetadataObject()
        {
            Handler.WrapperLookup.Remove(MetadataObject);
            MetadataObject = MetadataObject.Clone() as TOM.Perspective;
            Handler.WrapperLookup.Add(MetadataObject, this);
        }

		public Model Parent { 
			get {
				return Handler.WrapperLookup[MetadataObject.Parent] as Model;
			}
		}



		/// <summary>
		/// CTOR - only called from static factory methods on the class
		/// </summary>
		protected Perspective(TOM.Perspective metadataObject, bool init = true) : base(metadataObject)
		{
			if(init) InternalInit();
		}

		private void InternalInit()
		{
			// Create indexers for translations:
			TranslatedNames = new TranslationIndexer(this, TOM.TranslatedProperty.Caption);
			TranslatedDescriptions = new TranslationIndexer(this, TOM.TranslatedProperty.Description);
			
			// Create indexer for annotations:
			Annotations = new AnnotationCollection(this);
		}



		internal override void Undelete(ITabularObjectCollection collection) {
			base.Undelete(collection);
			Reinit();
			ReapplyReferences();
		}

		public override bool Browsable(string propertyName) {
			switch (propertyName) {
				case Properties.PARENT:
					return false;
				
				// Hides translation properties in the grid, unless the model actually contains translations:
				case Properties.TRANSLATEDNAMES:
				case Properties.TRANSLATEDDESCRIPTIONS:
					return Model.Cultures.Any();
				
				default:
					return base.Browsable(propertyName);
			}
		}

    }


	/// <summary>
	/// Collection class for Perspective. Provides convenient properties for setting a property on multiple objects at once.
	/// </summary>
	public partial class PerspectiveCollection: TabularObjectCollection<Perspective, TOM.Perspective, TOM.Model>
	{
		public override TabularNamedObject Parent { get { return Model; } }
		public PerspectiveCollection(string collectionName, TOM.PerspectiveCollection metadataObjectCollection, Model parent) : base(collectionName, metadataObjectCollection)
		{
		}

		internal override void Reinit() {
			var ixOffset = 0;
			for(int i = 0; i < Count; i++) {
				var item = this[i];
				Handler.WrapperLookup.Remove(item.MetadataObject);
				item.MetadataObject = Model.MetadataObject.Perspectives[i + ixOffset] as TOM.Perspective;
				Handler.WrapperLookup.Add(item.MetadataObject, item);
				item.Collection = this;
			}
			MetadataObjectCollection = Model.MetadataObject.Perspectives;
			foreach(var item in this) item.Reinit();
		}

		internal override void ReapplyReferences() {
			foreach(var item in this) item.ReapplyReferences();
		}

		/// <summary>
		/// Calling this method will populate the PerspectiveCollection with objects based on the MetadataObjects in the corresponding MetadataObjectCollection.
		/// </summary>
		public override void CreateChildrenFromMetadata()
		{
			// Construct child objects (they are automatically added to the Handler's WrapperLookup dictionary):
			foreach(var obj in MetadataObjectCollection) {
				Perspective.CreateFromMetadata(obj).Collection = this;
			}
		}

		[Description("Sets the Description property of all objects in the collection at once.")]
		public string Description {
			set {
				if(Handler == null) return;
				Handler.UndoManager.BeginBatch(UndoPropertyChangedAction.GetActionNameFromProperty("Description"));
				this.ToList().ForEach(item => { item.Description = value; });
				Handler.UndoManager.EndBatch();
			}
		}

		public override string ToString() {
			return string.Format("({0} {1})", Count, (Count == 1 ? "Perspective" : "Perspectives").ToLower());
		}
	}
}
