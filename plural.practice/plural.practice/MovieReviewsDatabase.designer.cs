﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17020
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace plural.practice
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="MovieReviews")]
	public partial class MovieReviewsDatabaseDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertMovie_(Movie_ instance);
    partial void UpdateMovie_(Movie_ instance);
    partial void DeleteMovie_(Movie_ instance);
    partial void InsertReview_(Review_ instance);
    partial void UpdateReview_(Review_ instance);
    partial void DeleteReview_(Review_ instance);
    #endregion
		
		public MovieReviewsDatabaseDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public MovieReviewsDatabaseDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public MovieReviewsDatabaseDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public MovieReviewsDatabaseDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Movie_> Movie_s
		{
			get
			{
				return this.GetTable<Movie_>();
			}
		}
		
		public System.Data.Linq.Table<Review_> Review_s
		{
			get
			{
				return this.GetTable<Review_>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Movies")]
	public partial class Movie_ : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _ID;
		
		private string _Title;
		
		private System.DateTime _ReleaseDate;
		
		private EntitySet<Review_> _Review_s;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(int value);
    partial void OnIDChanged();
    partial void OnTitleChanging(string value);
    partial void OnTitleChanged();
    partial void OnReleaseDateChanging(System.DateTime value);
    partial void OnReleaseDateChanged();
    #endregion
		
		public Movie_()
		{
			this._Review_s = new EntitySet<Review_>(new Action<Review_>(this.attach_Review_s), new Action<Review_>(this.detach_Review_s));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Title", DbType="NVarChar(MAX)")]
		public string Title
		{
			get
			{
				return this._Title;
			}
			set
			{
				if ((this._Title != value))
				{
					this.OnTitleChanging(value);
					this.SendPropertyChanging();
					this._Title = value;
					this.SendPropertyChanged("Title");
					this.OnTitleChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ReleaseDate", DbType="DateTime NOT NULL")]
		public System.DateTime ReleaseDate
		{
			get
			{
				return this._ReleaseDate;
			}
			set
			{
				if ((this._ReleaseDate != value))
				{
					this.OnReleaseDateChanging(value);
					this.SendPropertyChanging();
					this._ReleaseDate = value;
					this.SendPropertyChanged("ReleaseDate");
					this.OnReleaseDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Movy_Review", Storage="_Review_s", ThisKey="ID", OtherKey="Movie_ID")]
		public EntitySet<Review_> Review_s
		{
			get
			{
				return this._Review_s;
			}
			set
			{
				this._Review_s.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_Review_s(Review_ entity)
		{
			this.SendPropertyChanging();
			entity.Movie_ = this;
		}
		
		private void detach_Review_s(Review_ entity)
		{
			this.SendPropertyChanging();
			entity.Movie_ = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Reviews")]
	public partial class Review_ : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _ReviewID;
		
		private string _Summary;
		
		private int _Rating;
		
		private string _Body;
		
		private string _Reviewer;
		
		private System.Nullable<int> _Movie_ID;
		
		private EntityRef<Movie_> _Movie_;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnReviewIDChanging(int value);
    partial void OnReviewIDChanged();
    partial void OnSummaryChanging(string value);
    partial void OnSummaryChanged();
    partial void OnRatingChanging(int value);
    partial void OnRatingChanged();
    partial void OnBodyChanging(string value);
    partial void OnBodyChanged();
    partial void OnReviewerChanging(string value);
    partial void OnReviewerChanged();
    partial void OnMovie_IDChanging(System.Nullable<int> value);
    partial void OnMovie_IDChanged();
    #endregion
		
		public Review_()
		{
			this._Movie_ = default(EntityRef<Movie_>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ReviewID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int ReviewID
		{
			get
			{
				return this._ReviewID;
			}
			set
			{
				if ((this._ReviewID != value))
				{
					this.OnReviewIDChanging(value);
					this.SendPropertyChanging();
					this._ReviewID = value;
					this.SendPropertyChanged("ReviewID");
					this.OnReviewIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Summary", DbType="NVarChar(MAX)")]
		public string Summary
		{
			get
			{
				return this._Summary;
			}
			set
			{
				if ((this._Summary != value))
				{
					this.OnSummaryChanging(value);
					this.SendPropertyChanging();
					this._Summary = value;
					this.SendPropertyChanged("Summary");
					this.OnSummaryChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Rating", DbType="Int NOT NULL")]
		public int Rating
		{
			get
			{
				return this._Rating;
			}
			set
			{
				if ((this._Rating != value))
				{
					this.OnRatingChanging(value);
					this.SendPropertyChanging();
					this._Rating = value;
					this.SendPropertyChanged("Rating");
					this.OnRatingChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Body", DbType="NVarChar(MAX)")]
		public string Body
		{
			get
			{
				return this._Body;
			}
			set
			{
				if ((this._Body != value))
				{
					this.OnBodyChanging(value);
					this.SendPropertyChanging();
					this._Body = value;
					this.SendPropertyChanged("Body");
					this.OnBodyChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Reviewer", DbType="NVarChar(MAX)")]
		public string Reviewer
		{
			get
			{
				return this._Reviewer;
			}
			set
			{
				if ((this._Reviewer != value))
				{
					this.OnReviewerChanging(value);
					this.SendPropertyChanging();
					this._Reviewer = value;
					this.SendPropertyChanged("Reviewer");
					this.OnReviewerChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Movie_ID", DbType="Int")]
		public System.Nullable<int> Movie_ID
		{
			get
			{
				return this._Movie_ID;
			}
			set
			{
				if ((this._Movie_ID != value))
				{
					if (this._Movie_.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnMovie_IDChanging(value);
					this.SendPropertyChanging();
					this._Movie_ID = value;
					this.SendPropertyChanged("Movie_ID");
					this.OnMovie_IDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Movy_Review", Storage="_Movie_", ThisKey="Movie_ID", OtherKey="ID", IsForeignKey=true)]
		public Movie_ Movie_
		{
			get
			{
				return this._Movie_.Entity;
			}
			set
			{
				Movie_ previousValue = this._Movie_.Entity;
				if (((previousValue != value) 
							|| (this._Movie_.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Movie_.Entity = null;
						previousValue.Review_s.Remove(this);
					}
					this._Movie_.Entity = value;
					if ((value != null))
					{
						value.Review_s.Add(this);
						this._Movie_ID = value.ID;
					}
					else
					{
						this._Movie_ID = default(Nullable<int>);
					}
					this.SendPropertyChanged("Movie_");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591