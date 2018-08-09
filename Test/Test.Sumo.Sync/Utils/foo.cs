using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GSP.X.Repository.Local
{
    public class SyncStatus : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private System.Double _syncAPI;
		public System.Double SyncAPI
		{
			get { return _syncAPI; }
			set
			{
				if(_syncAPI != value)
				{
					_syncAPI = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.DateTime _syncDate;
		public System.DateTime SyncDate
		{
			get { return _syncDate; }
			set
			{
				if(_syncDate != value)
				{
					_syncDate = value;
					NotifyPropertyChanged();
				}
			}
		}

	}

	public class Stores : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private System.Int32 _storeId;
		public System.Int32 StoreId
		{
			get { return _storeId; }
			set
			{
				if(_storeId != value)
				{
					_storeId = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _storeName;
		public string StoreName
		{
			get { return _storeName; }
			set
			{
				if(_storeName != value)
				{
					_storeName = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _manager;
		public string Manager
		{
			get { return _manager; }
			set
			{
				if(_manager != value)
				{
					_manager = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Int32 _storeNr;
		public System.Int32 StoreNr
		{
			get { return _storeNr; }
			set
			{
				if(_storeNr != value)
				{
					_storeNr = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _address1;
		public string Address1
		{
			get { return _address1; }
			set
			{
				if(_address1 != value)
				{
					_address1 = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _address2;
		public string Address2
		{
			get { return _address2; }
			set
			{
				if(_address2 != value)
				{
					_address2 = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _city;
		public string City
		{
			get { return _city; }
			set
			{
				if(_city != value)
				{
					_city = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _state;
		public string State
		{
			get { return _state; }
			set
			{
				if(_state != value)
				{
					_state = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _zip;
		public string Zip
		{
			get { return _zip; }
			set
			{
				if(_zip != value)
				{
					_zip = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _county;
		public string County
		{
			get { return _county; }
			set
			{
				if(_county != value)
				{
					_county = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _country;
		public string Country
		{
			get { return _country; }
			set
			{
				if(_country != value)
				{
					_country = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Int32 _createdBy;
		public System.Int32 CreatedBy
		{
			get { return _createdBy; }
			set
			{
				if(_createdBy != value)
				{
					_createdBy = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.DateTime _createdOn;
		public System.DateTime CreatedOn
		{
			get { return _createdOn; }
			set
			{
				if(_createdOn != value)
				{
					_createdOn = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Int32 _modifiedBy;
		public System.Int32 ModifiedBy
		{
			get { return _modifiedBy; }
			set
			{
				if(_modifiedBy != value)
				{
					_modifiedBy = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.DateTime _modifiedOn;
		public System.DateTime ModifiedOn
		{
			get { return _modifiedOn; }
			set
			{
				if(_modifiedOn != value)
				{
					_modifiedOn = value;
					NotifyPropertyChanged();
				}
			}
		}

	}

	public class Surveys : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private System.Int32 _surveyId;
		public System.Int32 SurveyId
		{
			get { return _surveyId; }
			set
			{
				if(_surveyId != value)
				{
					_surveyId = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Int32 _surveyCategoryId;
		public System.Int32 SurveyCategoryId
		{
			get { return _surveyCategoryId; }
			set
			{
				if(_surveyCategoryId != value)
				{
					_surveyCategoryId = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _surveyName;
		public string SurveyName
		{
			get { return _surveyName; }
			set
			{
				if(_surveyName != value)
				{
					_surveyName = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _surveyDescription;
		public string SurveyDescription
		{
			get { return _surveyDescription; }
			set
			{
				if(_surveyDescription != value)
				{
					_surveyDescription = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _confirmationMessage;
		public string ConfirmationMessage
		{
			get { return _confirmationMessage; }
			set
			{
				if(_confirmationMessage != value)
				{
					_confirmationMessage = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Boolean _displayNavBar;
		public System.Boolean DisplayNavBar
		{
			get { return _displayNavBar; }
			set
			{
				if(_displayNavBar != value)
				{
					_displayNavBar = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Boolean _isAuditTemplate;
		public System.Boolean IsAuditTemplate
		{
			get { return _isAuditTemplate; }
			set
			{
				if(_isAuditTemplate != value)
				{
					_isAuditTemplate = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _createdBy;
		public string CreatedBy
		{
			get { return _createdBy; }
			set
			{
				if(_createdBy != value)
				{
					_createdBy = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.DateTime _createdOn;
		public System.DateTime CreatedOn
		{
			get { return _createdOn; }
			set
			{
				if(_createdOn != value)
				{
					_createdOn = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _modifiedBy;
		public string ModifiedBy
		{
			get { return _modifiedBy; }
			set
			{
				if(_modifiedBy != value)
				{
					_modifiedBy = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.DateTime _modifiedOn;
		public System.DateTime ModifiedOn
		{
			get { return _modifiedOn; }
			set
			{
				if(_modifiedOn != value)
				{
					_modifiedOn = value;
					NotifyPropertyChanged();
				}
			}
		}

	}

	public class SurveyCategories : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private System.Int32 _surveyCategoryId;
		public System.Int32 SurveyCategoryId
		{
			get { return _surveyCategoryId; }
			set
			{
				if(_surveyCategoryId != value)
				{
					_surveyCategoryId = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _surveyCategoryName;
		public string SurveyCategoryName
		{
			get { return _surveyCategoryName; }
			set
			{
				if(_surveyCategoryName != value)
				{
					_surveyCategoryName = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _createdBy;
		public string CreatedBy
		{
			get { return _createdBy; }
			set
			{
				if(_createdBy != value)
				{
					_createdBy = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.DateTime _createdOn;
		public System.DateTime CreatedOn
		{
			get { return _createdOn; }
			set
			{
				if(_createdOn != value)
				{
					_createdOn = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _modifiedBy;
		public string ModifiedBy
		{
			get { return _modifiedBy; }
			set
			{
				if(_modifiedBy != value)
				{
					_modifiedBy = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.DateTime _modifiedOn;
		public System.DateTime ModifiedOn
		{
			get { return _modifiedOn; }
			set
			{
				if(_modifiedOn != value)
				{
					_modifiedOn = value;
					NotifyPropertyChanged();
				}
			}
		}

	}

	public class SurveySections : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private System.Int32 _surveySectionId;
		public System.Int32 SurveySectionId
		{
			get { return _surveySectionId; }
			set
			{
				if(_surveySectionId != value)
				{
					_surveySectionId = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Int32 _surveyId;
		public System.Int32 SurveyId
		{
			get { return _surveyId; }
			set
			{
				if(_surveyId != value)
				{
					_surveyId = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _surveySectionName;
		public string SurveySectionName
		{
			get { return _surveySectionName; }
			set
			{
				if(_surveySectionName != value)
				{
					_surveySectionName = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _color;
		public string Color
		{
			get { return _color; }
			set
			{
				if(_color != value)
				{
					_color = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Int32 _sortOrder;
		public System.Int32 SortOrder
		{
			get { return _sortOrder; }
			set
			{
				if(_sortOrder != value)
				{
					_sortOrder = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _createdBy;
		public string CreatedBy
		{
			get { return _createdBy; }
			set
			{
				if(_createdBy != value)
				{
					_createdBy = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.DateTime _createdOn;
		public System.DateTime CreatedOn
		{
			get { return _createdOn; }
			set
			{
				if(_createdOn != value)
				{
					_createdOn = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _modifiedBy;
		public string ModifiedBy
		{
			get { return _modifiedBy; }
			set
			{
				if(_modifiedBy != value)
				{
					_modifiedBy = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.DateTime _modifiedOn;
		public System.DateTime ModifiedOn
		{
			get { return _modifiedOn; }
			set
			{
				if(_modifiedOn != value)
				{
					_modifiedOn = value;
					NotifyPropertyChanged();
				}
			}
		}

	}

	public class SurveyComments : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private System.Int32 _surveyCommentId;
		public System.Int32 SurveyCommentId
		{
			get { return _surveyCommentId; }
			set
			{
				if(_surveyCommentId != value)
				{
					_surveyCommentId = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Int32 _surveySectionId;
		public System.Int32 SurveySectionId
		{
			get { return _surveySectionId; }
			set
			{
				if(_surveySectionId != value)
				{
					_surveySectionId = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Int32 _surveyId;
		public System.Int32 SurveyId
		{
			get { return _surveyId; }
			set
			{
				if(_surveyId != value)
				{
					_surveyId = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _text;
		public string Text
		{
			get { return _text; }
			set
			{
				if(_text != value)
				{
					_text = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Int32 _createdBy;
		public System.Int32 CreatedBy
		{
			get { return _createdBy; }
			set
			{
				if(_createdBy != value)
				{
					_createdBy = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.DateTime _createdOn;
		public System.DateTime CreatedOn
		{
			get { return _createdOn; }
			set
			{
				if(_createdOn != value)
				{
					_createdOn = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Int32 _modifiedBy;
		public System.Int32 ModifiedBy
		{
			get { return _modifiedBy; }
			set
			{
				if(_modifiedBy != value)
				{
					_modifiedBy = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.DateTime _modifiedOn;
		public System.DateTime ModifiedOn
		{
			get { return _modifiedOn; }
			set
			{
				if(_modifiedOn != value)
				{
					_modifiedOn = value;
					NotifyPropertyChanged();
				}
			}
		}

	}

	public class SurveyQuestions : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private System.Int32 _surveyQuestionId;
		public System.Int32 SurveyQuestionId
		{
			get { return _surveyQuestionId; }
			set
			{
				if(_surveyQuestionId != value)
				{
					_surveyQuestionId = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Int32 _surveySectionId;
		public System.Int32 SurveySectionId
		{
			get { return _surveySectionId; }
			set
			{
				if(_surveySectionId != value)
				{
					_surveySectionId = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Int32 _surveyId;
		public System.Int32 SurveyId
		{
			get { return _surveyId; }
			set
			{
				if(_surveyId != value)
				{
					_surveyId = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _question;
		public string Question
		{
			get { return _question; }
			set
			{
				if(_question != value)
				{
					_question = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Int32 _questionType;
		public System.Int32 QuestionType
		{
			get { return _questionType; }
			set
			{
				if(_questionType != value)
				{
					_questionType = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Boolean _requireMedia;
		public System.Boolean RequireMedia
		{
			get { return _requireMedia; }
			set
			{
				if(_requireMedia != value)
				{
					_requireMedia = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Boolean _allowBlank;
		public System.Boolean AllowBlank
		{
			get { return _allowBlank; }
			set
			{
				if(_allowBlank != value)
				{
					_allowBlank = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Boolean _allowComments;
		public System.Boolean AllowComments
		{
			get { return _allowComments; }
			set
			{
				if(_allowComments != value)
				{
					_allowComments = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Int32 _minValue;
		public System.Int32 MinValue
		{
			get { return _minValue; }
			set
			{
				if(_minValue != value)
				{
					_minValue = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Boolean _autoCreate;
		public System.Boolean AutoCreate
		{
			get { return _autoCreate; }
			set
			{
				if(_autoCreate != value)
				{
					_autoCreate = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _supportLink;
		public string SupportLink
		{
			get { return _supportLink; }
			set
			{
				if(_supportLink != value)
				{
					_supportLink = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Int32 _maxValue;
		public System.Int32 MaxValue
		{
			get { return _maxValue; }
			set
			{
				if(_maxValue != value)
				{
					_maxValue = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Int32 _controlType;
		public System.Int32 ControlType
		{
			get { return _controlType; }
			set
			{
				if(_controlType != value)
				{
					_controlType = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _dataType;
		public string DataType
		{
			get { return _dataType; }
			set
			{
				if(_dataType != value)
				{
					_dataType = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Int32 _formFieldId;
		public System.Int32 FormFieldId
		{
			get { return _formFieldId; }
			set
			{
				if(_formFieldId != value)
				{
					_formFieldId = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Boolean _includeNotApplicable;
		public System.Boolean IncludeNotApplicable
		{
			get { return _includeNotApplicable; }
			set
			{
				if(_includeNotApplicable != value)
				{
					_includeNotApplicable = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Boolean _requireCommentsIfNotApplicable;
		public System.Boolean RequireCommentsIfNotApplicable
		{
			get { return _requireCommentsIfNotApplicable; }
			set
			{
				if(_requireCommentsIfNotApplicable != value)
				{
					_requireCommentsIfNotApplicable = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Int32 _sortOrder;
		public System.Int32 SortOrder
		{
			get { return _sortOrder; }
			set
			{
				if(_sortOrder != value)
				{
					_sortOrder = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Boolean _isExtraCredit;
		public System.Boolean IsExtraCredit
		{
			get { return _isExtraCredit; }
			set
			{
				if(_isExtraCredit != value)
				{
					_isExtraCredit = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Boolean _isScoreable;
		public System.Boolean IsScoreable
		{
			get { return _isScoreable; }
			set
			{
				if(_isScoreable != value)
				{
					_isScoreable = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Boolean _isStoreType;
		public System.Boolean IsStoreType
		{
			get { return _isStoreType; }
			set
			{
				if(_isStoreType != value)
				{
					_isStoreType = value;
					NotifyPropertyChanged();
				}
			}
		}

	}

	public class SurveyQuestionAnswers : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private System.Int32 _surveyQuestionAnswerId;
		public System.Int32 SurveyQuestionAnswerId
		{
			get { return _surveyQuestionAnswerId; }
			set
			{
				if(_surveyQuestionAnswerId != value)
				{
					_surveyQuestionAnswerId = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Int32 _surveyQuestionId;
		public System.Int32 SurveyQuestionId
		{
			get { return _surveyQuestionId; }
			set
			{
				if(_surveyQuestionId != value)
				{
					_surveyQuestionId = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _text;
		public string Text
		{
			get { return _text; }
			set
			{
				if(_text != value)
				{
					_text = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _type;
		public string Type
		{
			get { return _type; }
			set
			{
				if(_type != value)
				{
					_type = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Int32 _sortOrder;
		public System.Int32 SortOrder
		{
			get { return _sortOrder; }
			set
			{
				if(_sortOrder != value)
				{
					_sortOrder = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Int32 _surveyImageId;
		public System.Int32 SurveyImageId
		{
			get { return _surveyImageId; }
			set
			{
				if(_surveyImageId != value)
				{
					_surveyImageId = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Int32 _textValueItemId;
		public System.Int32 TextValueItemId
		{
			get { return _textValueItemId; }
			set
			{
				if(_textValueItemId != value)
				{
					_textValueItemId = value;
					NotifyPropertyChanged();
				}
			}
		}

	}

	public class SurveyQuestionImages : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private System.Int32 _surveyQuestionImageId;
		public System.Int32 SurveyQuestionImageId
		{
			get { return _surveyQuestionImageId; }
			set
			{
				if(_surveyQuestionImageId != value)
				{
					_surveyQuestionImageId = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Int32 _surveyId;
		public System.Int32 SurveyId
		{
			get { return _surveyId; }
			set
			{
				if(_surveyId != value)
				{
					_surveyId = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Int32 _surveyQuestionId;
		public System.Int32 SurveyQuestionId
		{
			get { return _surveyQuestionId; }
			set
			{
				if(_surveyQuestionId != value)
				{
					_surveyQuestionId = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _label;
		public string Label
		{
			get { return _label; }
			set
			{
				if(_label != value)
				{
					_label = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Int32 _mediaId;
		public System.Int32 MediaId
		{
			get { return _mediaId; }
			set
			{
				if(_mediaId != value)
				{
					_mediaId = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Int32 _sortOrder;
		public System.Int32 SortOrder
		{
			get { return _sortOrder; }
			set
			{
				if(_sortOrder != value)
				{
					_sortOrder = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _filename;
		public string Filename
		{
			get { return _filename; }
			set
			{
				if(_filename != value)
				{
					_filename = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _extension;
		public string Extension
		{
			get { return _extension; }
			set
			{
				if(_extension != value)
				{
					_extension = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Decimal _latitude;
		public System.Decimal Latitude
		{
			get { return _latitude; }
			set
			{
				if(_latitude != value)
				{
					_latitude = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Decimal _longitude;
		public System.Decimal Longitude
		{
			get { return _longitude; }
			set
			{
				if(_longitude != value)
				{
					_longitude = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Int32 _fileSize;
		public System.Int32 FileSize
		{
			get { return _fileSize; }
			set
			{
				if(_fileSize != value)
				{
					_fileSize = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Int32 _width;
		public System.Int32 Width
		{
			get { return _width; }
			set
			{
				if(_width != value)
				{
					_width = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Int32 _height;
		public System.Int32 Height
		{
			get { return _height; }
			set
			{
				if(_height != value)
				{
					_height = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _description;
		public string Description
		{
			get { return _description; }
			set
			{
				if(_description != value)
				{
					_description = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Int32 _createdBy;
		public System.Int32 CreatedBy
		{
			get { return _createdBy; }
			set
			{
				if(_createdBy != value)
				{
					_createdBy = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.DateTime _createdOn;
		public System.DateTime CreatedOn
		{
			get { return _createdOn; }
			set
			{
				if(_createdOn != value)
				{
					_createdOn = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Int32 _modifiedBy;
		public System.Int32 ModifiedBy
		{
			get { return _modifiedBy; }
			set
			{
				if(_modifiedBy != value)
				{
					_modifiedBy = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.DateTime _modifiedOn;
		public System.DateTime ModifiedOn
		{
			get { return _modifiedOn; }
			set
			{
				if(_modifiedOn != value)
				{
					_modifiedOn = value;
					NotifyPropertyChanged();
				}
			}
		}

	}

	public class SurveyEvents : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private System.Int32 _surveyEventId;
		public System.Int32 SurveyEventId
		{
			get { return _surveyEventId; }
			set
			{
				if(_surveyEventId != value)
				{
					_surveyEventId = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Int32 _surveyEventRecipientId;
		public System.Int32 SurveyEventRecipientId
		{
			get { return _surveyEventRecipientId; }
			set
			{
				if(_surveyEventRecipientId != value)
				{
					_surveyEventRecipientId = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Int32 _surveyId;
		public System.Int32 SurveyId
		{
			get { return _surveyId; }
			set
			{
				if(_surveyId != value)
				{
					_surveyId = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Boolean _isAuditEvent;
		public System.Boolean IsAuditEvent
		{
			get { return _isAuditEvent; }
			set
			{
				if(_isAuditEvent != value)
				{
					_isAuditEvent = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Boolean _isStoreSurvey;
		public System.Boolean IsStoreSurvey
		{
			get { return _isStoreSurvey; }
			set
			{
				if(_isStoreSurvey != value)
				{
					_isStoreSurvey = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Int32 _distributionType;
		public System.Int32 DistributionType
		{
			get { return _distributionType; }
			set
			{
				if(_distributionType != value)
				{
					_distributionType = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _eventName;
		public string EventName
		{
			get { return _eventName; }
			set
			{
				if(_eventName != value)
				{
					_eventName = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _eventDescription;
		public string EventDescription
		{
			get { return _eventDescription; }
			set
			{
				if(_eventDescription != value)
				{
					_eventDescription = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.DateTime _startOn;
		public System.DateTime StartOn
		{
			get { return _startOn; }
			set
			{
				if(_startOn != value)
				{
					_startOn = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.DateTime _endOn;
		public System.DateTime EndOn
		{
			get { return _endOn; }
			set
			{
				if(_endOn != value)
				{
					_endOn = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Boolean _restrictToDates;
		public System.Boolean RestrictToDates
		{
			get { return _restrictToDates; }
			set
			{
				if(_restrictToDates != value)
				{
					_restrictToDates = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _email;
		public string Email
		{
			get { return _email; }
			set
			{
				if(_email != value)
				{
					_email = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.DateTime _sentOn;
		public System.DateTime SentOn
		{
			get { return _sentOn; }
			set
			{
				if(_sentOn != value)
				{
					_sentOn = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Int32 _createdBy;
		public System.Int32 CreatedBy
		{
			get { return _createdBy; }
			set
			{
				if(_createdBy != value)
				{
					_createdBy = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.DateTime _createdOn;
		public System.DateTime CreatedOn
		{
			get { return _createdOn; }
			set
			{
				if(_createdOn != value)
				{
					_createdOn = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Int32 _modifiedBy;
		public System.Int32 ModifiedBy
		{
			get { return _modifiedBy; }
			set
			{
				if(_modifiedBy != value)
				{
					_modifiedBy = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.DateTime _modifiedOn;
		public System.DateTime ModifiedOn
		{
			get { return _modifiedOn; }
			set
			{
				if(_modifiedOn != value)
				{
					_modifiedOn = value;
					NotifyPropertyChanged();
				}
			}
		}

	}

	public class SurveyEventRespondents : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private System.Int32 _surveyEventRespondentId;
		public System.Int32 SurveyEventRespondentId
		{
			get { return _surveyEventRespondentId; }
			set
			{
				if(_surveyEventRespondentId != value)
				{
					_surveyEventRespondentId = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Int32 _surveyEventId;
		public System.Int32 SurveyEventId
		{
			get { return _surveyEventId; }
			set
			{
				if(_surveyEventId != value)
				{
					_surveyEventId = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.DateTime _completedOn;
		public System.DateTime CompletedOn
		{
			get { return _completedOn; }
			set
			{
				if(_completedOn != value)
				{
					_completedOn = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _identifier;
		public string Identifier
		{
			get { return _identifier; }
			set
			{
				if(_identifier != value)
				{
					_identifier = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.DateTime _createdOn;
		public System.DateTime CreatedOn
		{
			get { return _createdOn; }
			set
			{
				if(_createdOn != value)
				{
					_createdOn = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.DateTime _modifiedOn;
		public System.DateTime ModifiedOn
		{
			get { return _modifiedOn; }
			set
			{
				if(_modifiedOn != value)
				{
					_modifiedOn = value;
					NotifyPropertyChanged();
				}
			}
		}

	}

	public class SurveyEventRespondentAnswers : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private System.Int32 _surveyEventRespondentAnswerId;
		public System.Int32 SurveyEventRespondentAnswerId
		{
			get { return _surveyEventRespondentAnswerId; }
			set
			{
				if(_surveyEventRespondentAnswerId != value)
				{
					_surveyEventRespondentAnswerId = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Int32 _surveyEventRespondentId;
		public System.Int32 SurveyEventRespondentId
		{
			get { return _surveyEventRespondentId; }
			set
			{
				if(_surveyEventRespondentId != value)
				{
					_surveyEventRespondentId = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Int32 _surveyQuestionId;
		public System.Int32 SurveyQuestionId
		{
			get { return _surveyQuestionId; }
			set
			{
				if(_surveyQuestionId != value)
				{
					_surveyQuestionId = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Boolean _isNotApplicable;
		public System.Boolean IsNotApplicable
		{
			get { return _isNotApplicable; }
			set
			{
				if(_isNotApplicable != value)
				{
					_isNotApplicable = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _comment;
		public string Comment
		{
			get { return _comment; }
			set
			{
				if(_comment != value)
				{
					_comment = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string _answer;
		public string Answer
		{
			get { return _answer; }
			set
			{
				if(_answer != value)
				{
					_answer = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Boolean _isApproved;
		public System.Boolean IsApproved
		{
			get { return _isApproved; }
			set
			{
				if(_isApproved != value)
				{
					_isApproved = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Boolean _isImported;
		public System.Boolean IsImported
		{
			get { return _isImported; }
			set
			{
				if(_isImported != value)
				{
					_isImported = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Boolean _isFailed;
		public System.Boolean IsFailed
		{
			get { return _isFailed; }
			set
			{
				if(_isFailed != value)
				{
					_isFailed = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.Boolean _isBookmark;
		public System.Boolean IsBookmark
		{
			get { return _isBookmark; }
			set
			{
				if(_isBookmark != value)
				{
					_isBookmark = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.DateTime _createdOn;
		public System.DateTime CreatedOn
		{
			get { return _createdOn; }
			set
			{
				if(_createdOn != value)
				{
					_createdOn = value;
					NotifyPropertyChanged();
				}
			}
		}

		private System.DateTime _modifiedOn;
		public System.DateTime ModifiedOn
		{
			get { return _modifiedOn; }
			set
			{
				if(_modifiedOn != value)
				{
					_modifiedOn = value;
					NotifyPropertyChanged();
				}
			}
		}

	}

}
