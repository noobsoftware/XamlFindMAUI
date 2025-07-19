using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.Maui.Controls;
//using Plugin.Screenshot;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
//using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Layouts;

namespace noobtorrent {
    public class base_page : Microsoft.Maui.Controls.ContentPage {
	
        private List<Element> traversed = new List<Element>();
        private List<string> exclude_properties = new List<string>() {
          "Parent",
          "BindingContext",
          "ContentLayout",
          "Content",
          "Font",
          "FontFamily",
          "FontAttributes",
          "FontSize",
          "Style",
          "Resources",
          "class",
          "TextDecorations"
        };
        private int selector_index = 0;
        private bool capture_children = false;
        private bool immiediate_children = false;
        private string type_sel;
        private string class_sel;
        private List<Element> return_list;
        private List<selector_pair> selector_path;

        public Element copy(Element element) {
            Element element1 = (Element)new Microsoft.Maui.Controls.Grid();
            string str1 = element.ToString();
            switch(str1)  {
                /*case "MR.Gestures.Button":
                    element1 = (Element)new MR.Gestures.Button();
                    break;
                case "MR.Gestures.StackLayout":
                    element1 = (Element)new MR.Gestures.StackLayout();
                    break;*/
				case "Microsoft.Maui.Controls.SwitchCell":
                    element1 = (Element)new Microsoft.Maui.Controls.SwitchCell();
                    break;
                case "Microsoft.Maui.Controls.TextCell":
                    element1 = (Element)new Microsoft.Maui.Controls.TextCell();
                    break;
                case "Microsoft.Maui.Controls.ImageCell":
                    element1 = (Element)new Microsoft.Maui.Controls.ImageCell();
                    break;
                case "Microsoft.Maui.Controls.EntryCell":
                    element1 = (Element)new Microsoft.Maui.Controls.EntryCell();
                    break;
                case "Microsoft.Maui.Controls.TableView":
                    element1 = (Element)new Microsoft.Maui.Controls.TableView();
                    break;
                case "Microsoft.Maui.Controls.Switch":
                    element1 = (Element)new Microsoft.Maui.Controls.Switch();
                    break;
                case "Microsoft.Maui.Controls.Stepper":
                    element1 = (Element)new Microsoft.Maui.Controls.Stepper();
                    break;
                case "Microsoft.Maui.Controls.Slider":
                    element1 = (Element)new Microsoft.Maui.Controls.Slider();
                    break;
                case "Microsoft.Maui.Controls.Image":
                    element1 = (Element)new Microsoft.Maui.Controls.Image();
                    break;
                case "Microsoft.Maui.Controls.AbsoluteLayout":
                    element1 = (Element)new Microsoft.Maui.Controls.AbsoluteLayout();
                    break;
                case "Microsoft.Maui.Controls.BoxView":
                    element1 = (Element)new Microsoft.Maui.Controls.BoxView();
                    break;
                case "Microsoft.Maui.Controls.Button":
                    element1 = (Element)new Microsoft.Maui.Controls.Button();
                    break;
                case "Microsoft.Maui.Controls.Entry":
                    element1 = (Element)new Microsoft.Maui.Controls.Entry();
                    break;
                case "Microsoft.Maui.Controls.Frame":
                    element1 = (Element)new Microsoft.Maui.Controls.Frame();
                    break;
                case "Microsoft.Maui.Controls.Grid":
                    element1 = (Element)new Microsoft.Maui.Controls.Grid();
                    break;
                case "Microsoft.Maui.Controls.Label":
                    element1 = (Element)new Microsoft.Maui.Controls.Label();
                    break;
                case "Microsoft.Maui.Controls.StackLayout":
                    element1 = (Element)new Microsoft.Maui.Controls.StackLayout();
                    break;
                case "Microsoft.Maui.Controls.ScrollView":
                    element1 = (Element)new Microsoft.Maui.Controls.ScrollView();
                    break;
                case "Microsoft.Maui.Controls.Picker":
                    element1 = (Element)new Microsoft.Maui.Controls.Picker();
                    break;
                case "Microsoft.Maui.Controls.DatePicker":
                    element1 = (Element)new Microsoft.Maui.Controls.DatePicker();
                    break;
                case "Microsoft.Maui.Controls.TimePicker":
                    element1 = (Element)new Microsoft.Maui.Controls.TimePicker();
                    break;
                case "Microsoft.Maui.Controls.Editor":
                    element1 = (Element)new Microsoft.Maui.Controls.Editor();
                    break;
                case "Microsoft.Maui.Controls.FlexLayout":
                    element1 = (Element)new Microsoft.Maui.Controls.FlexLayout();
                    break;
                /*case "Microsoft.Maui.Controls.CheckBox":
                    element1 = (Element)new Microsoft.Maui.Controls.CheckBox();
                    break;
                */
				/*case "SkiaSharp.Views.Forms.SKCanvasView":
                    element1 = (Element)new SkiaSharp.Views.Forms.SKCanvasView();
					break;*/
            }
            foreach(PropertyInfo property in element.GetType().GetProperties()) {
                object obj = property.GetValue((object)element);
                if(property.CanWrite && !this.exclude_properties.Contains(property.Name)) {
					if(str1 == "MR.Gestures.Button") {
						if(property.Name.IndexOf("Command") == -1) {
							property.SetValue((object)element1, obj);

						}
					} else {
						property.SetValue((object)element1, obj);

					}
                }
            }
            /*if(str1 == "Microsoft.Maui.Controls.Label") {
                //Debug.WriteLine("inside font settings");
                Label l1 = (Label)element1;
                Label l = (Label)element;
                l1.FontAttributes = FontAttributes.None;
                //l1.FontAttributes = l.FontAttributes;
                l1.FontFamily = l.FontFamily;
                l1.FontSize = l.FontSize;
            }*/
            Rect layoutBounds = Microsoft.Maui.Controls.AbsoluteLayout.GetLayoutBounds((BindableObject)element);
            Microsoft.Maui.Controls.AbsoluteLayout.SetLayoutBounds((BindableObject)element1, layoutBounds);
            Microsoft.Maui.Controls.AbsoluteLayout.SetLayoutFlags((BindableObject)element1, Microsoft.Maui.Controls.AbsoluteLayout.GetLayoutFlags((BindableObject)element));
            if(str1 == "Microsoft.Maui.Controls.Grid") {
                Grid grid_element = (Grid)element;
                Grid grid_element1 = (Grid)element1;
                grid_element1.ColumnDefinitions = new ColumnDefinitionCollection();//.Clear();
                grid_element1.RowDefinitions = new RowDefinitionCollection();
                ColumnDefinitionCollection column_definitions = grid_element.ColumnDefinitions;
                RowDefinitionCollection row_definitions = grid_element.RowDefinitions;
                foreach(ColumnDefinition column_definition in column_definitions) {
                    grid_element1.ColumnDefinitions.Add(column_definition);
                }
                foreach(RowDefinition row_definition in row_definitions) {
                    grid_element1.RowDefinitions.Add(row_definition);
                }
                element1 = (Element)grid_element1;
            }
            foreach(Element child in this.children(element)) {
                View view = (View)this.copy(child);
                if(str1 == "Microsoft.Maui.Controls.Grid") {
                    int row = Microsoft.Maui.Controls.Grid.GetRow((BindableObject)child);
                    int column = Microsoft.Maui.Controls.Grid.GetColumn((BindableObject)child);
					int row_span = Microsoft.Maui.Controls.Grid.GetRowSpan(child);
					int col_span = Microsoft.Maui.Controls.Grid.GetColumnSpan(child);
                    Microsoft.Maui.Controls.Grid.SetRow((BindableObject)view, row);
                    Microsoft.Maui.Controls.Grid.SetColumn((BindableObject)view, column);
					Microsoft.Maui.Controls.Grid.SetRowSpan(view, row_span);
					Microsoft.Maui.Controls.Grid.SetColumnSpan(view, col_span);
                }
                foreach(FieldInfo field in element1.GetType().GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)) {
                    if(field.Name == "_logicalChildren") {
                        ReadOnlyCollection<Element> readOnlyCollection1 = (ReadOnlyCollection<Element>)field.GetValue((object)element1);
                        Collection<Element> collection = new Collection<Element>();
                        foreach(Element element2 in readOnlyCollection1)
                            collection.Add(element2);
                        collection.Add((Element)view);
                        ReadOnlyCollection<Element> readOnlyCollection2 = new ReadOnlyCollection<Element>((IList<Element>)collection);
                        field.SetValue((object)element1, (object)readOnlyCollection2);
                    } else if(field.Name == "_children") {
                        string str2 = str1;
                        if(!(str2 == "Microsoft.Maui.Controls.Grid")) {
                            if(str2 == "Microsoft.Maui.Controls.AbsoluteLayout") {
                                ((Microsoft.Maui.Controls.AbsoluteLayout)element1).Children.Add(view);
							}
                        } else {
                            ((Microsoft.Maui.Controls.Grid)element1).Children.Add(view);
						}
                    } else if(str1 == "Microsoft.Maui.Controls.Frame") {
                        ((Microsoft.Maui.Controls.ContentView)element1).Content = view;
                    } else if(str1 == "Microsoft.Maui.Controls.StackLayout") {
                        //((Layout<View>)element1).Children.Add(view);
                        ((Microsoft.Maui.Controls.StackLayout)element1).Children.Add(view);
                            //.Children.Add(view);
					} else if(str1 == "Microsoft.Maui.Controls.ScrollView") {
                        ((Microsoft.Maui.Controls.ScrollView)element1).Content = view;
                    } else if(str1 == "Microsoft.Maui.Controls.FlexLayout") {
                        ((Microsoft.Maui.Controls.FlexLayout)element1).Children.Add(view);
                    }
                }
            }
            return element1;
        }

        public void list_fields(Element e, string name=null) {
            foreach(MemberInfo field in e.GetType().GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)) {
				//if(field.Name.IndexOf(name) != -1) {
	                Debug.WriteLine("field: "+field.Name);
					//string value = (String)field.GetValue(e);
				//}
			}
            foreach(PropertyInfo property in e.GetType().GetProperties()) {
				//if(property.Name.IndexOf(name) != -1) {
					Debug.WriteLine("property: "+property.Name);
					/*Command value = (Command)property.GetValue(e);
					 
					Debug.WriteLine("value: "+value.ToString());*/
				//}
			}
        }

        public List<Element> _(string selector) {
            return this.find((Element)NameScopeExtensions.FindByName<Microsoft.Maui.Controls.AbsoluteLayout>(this, "body_wrap"), selector);
        }

		public List<Element> _find(List<Element> elements, List<selector_split> selectors, string command=" ") {
			List<Element> next_result = new List<Element>();
			selector_split selector = selectors[0];
			selectors.RemoveAt(0);
            //System.Diagnostics.Debug.WriteLine("command: "+command);
            //System.Diagnostics.Debug.WriteLine("command: "+selector.selector);
			foreach(Element e in elements) {
				if(command == " ") {
					next_result.AddRange(this.find(e, selector.selector));
				} else if(command == ">") {
                    /*List<Element> intermediate_result = this.children(e);
                    foreach(Element child in intermediate_result) {
                        //if(this.has_class(child, 
                    }*/
                    next_result.AddRange(this.find(e, selector.selector, true));
				}
			}
            //System.Diagnostics.Debug.WriteLine("next_result_count: "+next_result.Count);
            foreach(Element e in next_result) {
                //System.Diagnostics.Debug.WriteLine("el styleid: "+e.StyleId);
            }
            //next_result = next_result.Distinct<Element>().ToList();
			if(selectors.Count == 0) {
				return next_result;
			}
			return this._find(next_result, selectors, selector.next_command);
		}

		List<selector_split> _find_selectors;

		public void split_selector(string selector) {
			selector = selector.Trim();
			List<string> split = new List<string>();
			int space_index = selector.IndexOf(' ');
			int arrow_index = selector.IndexOf('>');
			if(space_index == -1 && arrow_index == -1) {
				this._find_selectors.Add(new selector_split(null, selector));
			} else {
				int selected_index;
				bool is_space = true;
				string command = " ";
				if(arrow_index == -1 || space_index < arrow_index) {
					is_space = true;
					selected_index = space_index;
				} else {
					is_space = false;
					selected_index = arrow_index;
					command = ">";
				}
				string first_part = selector.Substring(0, selected_index).Trim();
				string second_part = selector.Substring(selected_index+1).Trim();
				this._find_selectors.Add(new selector_split(command, first_part));
				if(second_part.Length > 0) {
					this.split_selector(second_part);
				}
			}
		}

        public List<Element> find(Element element, string selector, bool select_immediate_children=false) {
            //System.Diagnostics.Debug.WriteLine("-------find------"+element);
            //System.Diagnostics.Debug.WriteLine("-------find------"+selector);
			selector = selector.Trim();
			if(selector.IndexOf('>') != -1 || selector.IndexOf(' ') != -1) {
				//List<string> selector_split = new List<string>();
				this._find_selectors = new List<selector_split>();
				/*List<string> commands = new List<string>();
				List<string> space_split = new List<string>(selector.Split(' '));
				foreach(string space_split_value in space_split) {
					commands.Add(" ");
					List<string> arrow_split = new List<string>(space_split_value.Split('>'));
					foreach(string arrow_split_value in arrow_split) {
						selector_split.Add(arrow_split_value);
					}
				}*/
                this.split_selector(selector);
				List<selector_split> _find_selectors = this._find_selectors;
                foreach(selector_split split_value in _find_selectors) {
                    //System.Diagnostics.Debug.WriteLine("find_selector: "+split_value.selector);
                    //System.Diagnostics.Debug.WriteLine("find_selector: "+split_value.next_command);
                }
				//_find_selectors.Reverse();
				return this._find(new List<Element>() { element }, _find_selectors);
			}

            this.skip_first_child = element;
            this.capture_children = false;
            this.immiediate_children = false;
			if(select_immediate_children) {
				this.immiediate_children = true;
			}
            this.selector_index = 0;
            this.selector_path = new List<selector_pair>();
            this.return_list = new List<Element>();
            this.traversed = new List<Element>();
            List<string> stringList1 = new List<string>();
            if(selector.IndexOf(' ') != -1) {
                stringList1 = new List<string>((IEnumerable<string>)selector.Split(' '));
            } else {
                stringList1.Add(selector);
			}
            foreach(string str in stringList1) {
                List<string> stringList2 = new List<string>();
                if(str.IndexOf('.') != -1) {
                    List<string> stringList3 = new List<string>((IEnumerable<string>)str.Split('.'));
                    this.selector_path.Add(new selector_pair(stringList3[0], stringList3[1]));
                } else {
                    this.selector_path.Add(new selector_pair(str, ""));
				}
            }
            selector_pair selectorPair = this.pop_selector(0);
            int selector_index = 1;
            if(this.selector_path.Count == 1) {
                selector_index = 0;
			}
            this.traverse(element, selectorPair.type_sel, selectorPair.class_sel, selector_index, select_immediate_children);
            return this.return_list;
        }

        private Element skip_first_child = null;

        private selector_pair pop_selector(int selector_index) {
            if(selector_index < this.selector_path.Count) {
                return this.selector_path[selector_index];
			}
            return (selector_pair)null;
        }

        public bool has_class(string class_sel, string class_val) {
            foreach(string class_name in this.classes(class_val)) {
                if(class_sel == class_name) {
                    return true;
				}
            }
            return false;
        }

        public bool has_class(Element e, string class_val) {
            return this.has_class(class_val, e.StyleId);
        }

		public List<string> classes(string class_val) {
            List<string> classes = new List<string>(class_val.Split(';'));
			classes = new List<string>(classes[0].Split('.'));
			return classes;
		}

		public List<string> classes(Element e) {
            List<string> classes = new List<string>(e.StyleId.Split(';'));
			classes = new List<string>(classes[0].Split('.'));
			return classes;
		}

		public void add_class(Element e, string class_name) {
			List<string> split = new List<string>(e.StyleId.Split(';'));
			string classes = split[0]+"."+class_name;
            Dictionary<string, string> element_data = this.element_data(e);
            classes += ";";
            foreach(KeyValuePair<string, string> pair in element_data) {
                classes += pair.Key+":"+pair.Value+";";
            }
            e.StyleId = classes;
		}

		public void remove_class(Element e, string class_name) {
			List<string> split = new List<string>(e.StyleId.Split(';'));
			string classes_string = split[0];
			string data_string = "";
			int counter = 0;
			foreach(string data in split) {
				if(counter > 0) {
					data_string += data+";";
				}
				counter++;
			}
			List<string> classes = new List<string>(classes_string.Split('.'));
			string return_classes = "";
			foreach(string class_string in classes) {
				if(class_string != class_name) {
					return_classes += "."+class_string;
				}
			}
			e.StyleId = return_classes+";"+data_string;
		}

		/*public List<string> split(string value, char delimiter) {
			List<string> return_list = new List<string>();
			if(value.IndexOf(delimiter) != -1) {
				return_list = new List<string>(value.Split(delimiter));
			} else {
				return_list.Add(value);
			}
			return return_list;
		}*/

        public Dictionary<string, string> element_data(Element e) {
            /*List<string> classes = new List<string>(e.StyleId.Split(';'));
            string data_string = classes[classes.Count-1];*/

			List<string> data = new List<string>(e.StyleId.Split(';'));
			Dictionary<string, string> return_data = new Dictionary<string, string>();
			
			//Debug.WriteLine("data_string: "+data_string);
			int counter = 0;
			foreach(string s in data) {
				if(counter > 0) {
					if(s.IndexOf(':') != -1) {
						List<string> split = new List<string>(s.Split(':'));
						return_data.Add(split[0], split[1]);
					}
				}
				counter++;
			}
			return return_data;
        }

		public void add_element_data(Element e, string key, string value) {
			if(e.StyleId.IndexOf(';') == -1) {
				e.StyleId += ";";
			}
			if(e.StyleId.Substring(e.StyleId.Length-1, 1) != ";") {
				e.StyleId += ";";
			}
			e.StyleId += key+":"+value+";";
		}

		public void edit_element_data(Element e, string key, string value) {
			if(e.StyleId.IndexOf(';') == -1) {
				this.add_element_data(e, key, value);
			} else {
				List<string> split = new List<string>(e.StyleId.Split(';'));
				string classes = split[0];
                Dictionary<string, string> element_data = this.element_data(e);
                if(element_data.ContainsKey(key)) {
                    element_data[key] = value;
                } else {
                    element_data.Add(key, value);
                }

                classes += ";";
                foreach(KeyValuePair<string, string> pair in element_data) {
                    classes += pair.Key+":"+pair.Value+";";
                }
                e.StyleId = classes;
			}
		}

        public List<Element> children(Element element) {
            this.selector_path = new List<selector_pair>();
            this.return_list = new List<Element>();
            this.traversed = new List<Element>();
            this.capture_children = false;
            this.immiediate_children = true;
            this.traverse(element, (string)null, (string)null, 0);
            return this.return_list;
        }

        protected void traverse(Element element, string type_sel, string class_sel, int selector_index, bool find_immediate_children=false) {
            if(!this.traversed.Contains(element)) {
                ////System.Diagnostics.Debug.WriteLine(tabs+"element: "+element+" styleid:" +element.StyleId);
				this.traversed.Add(element);
				FieldInfo[] fields = element.GetType().GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				string element_type = element.ToString();
				/*//System.Diagnostics.Debug.WriteLine(element.StyleId);
				//System.Diagnostics.Debug.WriteLine(element_type);*/
				bool flag = false;
				if(this.capture_children) {
					flag = true;
				}
				if(!this.immiediate_children || find_immediate_children) {
					if(type_sel != "" && class_sel != "" && element.StyleId != null) {
						if(element_type.IndexOf(type_sel) != -1 && this.has_class(class_sel, element.StyleId)) {
							flag = true;
						}
					} else if(type_sel != "" && class_sel == "") {
						if(element_type.IndexOf(type_sel) != -1) {
							flag = true;
						}
					} else if(class_sel != "" && element.StyleId != null && this.has_class(class_sel, element.StyleId)) {
						flag = true;
					}
				}
                bool traverse_children = false;
				if(flag) {
					//Debug.WriteLine("valid");
					selector_pair selectorPair = this.pop_selector(selector_index);
					if(selectorPair == null) {
                        //if(element != this.skip_first_child) {
    						this.return_list.Add(element);
                        //}
					} else if(this.selector_path.Count == 1) {
                        //if(element != this.skip_first_child) {
    						this.return_list.Add(element);
                        //}
						//this.traverse(element, selectorPair.type_sel, selectorPair.class_sel, selector_index, tabs+"\t");
                        traverse_children = true;
					} else {
						//selector_index++;
						//this.traverse(element, selectorPair.type_sel, selectorPair.class_sel, selector_index, tabs+"\t");
                        traverse_children = true;
					}
				}// else {
                if(((!flag || traverse_children) && !this.immiediate_children) || (this.immiediate_children && !flag)) {
					if(this.immiediate_children && !find_immediate_children) {
						this.capture_children = true;
					}
					foreach(FieldInfo fieldInfo in fields) {
						fieldInfo.GetValue(element);
						if(fieldInfo.Name == "_logicalChildren") {
							//List<Element> element_children = new List<Element>(fieldInfo.GetValue(element));
							//foreach(Element element1 in (List<Element>)fieldInfo.GetValue(element)) { //(object)
							foreach(Element element1 in (ReadOnlyCollection<Element>)fieldInfo.GetValue(element)) {
								this.traverse(element1, type_sel, class_sel, selector_index);
							}
						} else if(fieldInfo.Name == "_children") {
							if(!(element_type == "Microsoft.Maui.Controls.Grid")) {
								if(element_type == "Microsoft.Maui.Controls.AbsoluteLayout") {
									foreach(Element child in ((Microsoft.Maui.Controls.AbsoluteLayout)element).Children) {
										this.traverse(child, type_sel, class_sel, selector_index);
									}
								}
							} else {
								foreach(Element child in ((Microsoft.Maui.Controls.Grid)element).Children) {
									this.traverse(child, type_sel, class_sel, selector_index);
								}
							}
						} else if(element_type == "Microsoft.Maui.Controls.Frame") {
							this.traverse((Element)((Microsoft.Maui.Controls.ContentView)element).Content, type_sel, class_sel, selector_index);
						} else if(element_type == "Microsoft.Maui.Controls.StackLayout") {
							//foreach(Element child in (List<View>)((StackLayout)element).Children) {
							foreach(Element child in ((StackLayout)element).Children) {
								this.traverse(child, type_sel, class_sel, selector_index);
							}
						} else if(element_type == "Microsoft.Maui.Controls.ListView") {
							Microsoft.Maui.Controls.ListView listView = (Microsoft.Maui.Controls.ListView)element;
							PropertyInfo propertyInfo = listView.GetType().GetRuntimeProperties().FirstOrDefault<PropertyInfo>((Func<PropertyInfo, bool>)(info => info.Name == "TemplatedItems"));
							if(propertyInfo != null) {
								foreach(Microsoft.Maui.Controls.ViewCell viewCell in (IEnumerable<Cell>)(propertyInfo.GetValue(listView) as ITemplatedItemsList<Cell>)) {
									if(viewCell.BindingContext != null) {
										this.traverse((Element)viewCell, type_sel, class_sel, selector_index);
									}
								}
							}
						} else if(element_type == "Microsoft.Maui.Controls.ScrollView") {
							Microsoft.Maui.Controls.ScrollView scroll_view = (Microsoft.Maui.Controls.ScrollView)element;
							Element child = scroll_view.Content;
							this.traverse(child, type_sel, class_sel, selector_index);
						} else if(element_type == "Microsoft.Maui.Controls.FlexLayout") {
                            Microsoft.Maui.Controls.FlexLayout flex_layout = (Microsoft.Maui.Controls.FlexLayout)element;
                            foreach(Element child in flex_layout.Children) {
								this.traverse(child, type_sel, class_sel, selector_index);
							}
                        }
					}
				}
			}
        }

        protected Element get_parent(Element child, int level) {
            if(level == 0) {
                return child;
			}
            return this.get_parent(child.Parent, level - 1);
        }

        public void pushModalAsync(Microsoft.Maui.Controls.ContentPage page) {
            this.Navigation.PushModalAsync((Page)page);
        }
		
		//private templates templates;
		//public AbsoluteLayout body_wrap;
		//public StackLayout content;

		public base_page() {
			//this.templates = (Application.Current as App).get_templates();
			/*this.OnAppearing += (s, e) => {

			};*/
		}

		/*public void set_base_elements() {
			this.body_wrap = this.FindByName<AbsoluteLayout>("body_wrap");
			this.content = this.FindByName<StackLayout>("content");
			//System.Diagnostics.Debug.WriteLine("init base elements");
			Debug.WriteLine(this.body_wrap);
			Debug.WriteLine(this.content);
		}

		protected override void OnAppearing() {
			Debug.WriteLine("on appearing new");
			/*this.set_base_elements();
			this.init_blur();*/
		//}

		/*public templates get_templates() {
			return this.templates; 
		}*/

		
        /*public Easing ease_in_1 = new Easing(t => Math.Pow(t, 0.22222222));
        public Easing ease_in_2 = new Easing(t => Math.Pow(t, 0.1111111));
        public Easing ease_in_3 = new Easing(t => Math.Pow(t, 0.1113333));
        public Easing ease_in_4 = new Easing(t => Math.Pow(t, 0.3333333));

        public Easing ease_out_1 = new Easing(t => Math.Pow(t, 1.333333));
        public Easing ease_out_2 = new Easing(t => Math.Pow(t, 2));
        public Easing ease_out_3 = new Easing(t => Math.Pow(t, 2.5));
        public Easing ease_out_4 = new Easing(t => Math.Pow(t, 3));

        public Easing teal = new Easing(t => -Math.Log(t)*Math.Sin(Math.Pow(t, 2)));

        public Easing log = new Easing(t => Math.Log(t));*/

		public async void display_element(View v, uint time=100, Easing ease=null) { //=this.ease_in_4
			await v.FadeTo(0, 0);
			v.IsVisible = true;
			await v.FadeTo(1, time, ease);
		}

		public async void hide_element(View v, uint time=100, Easing ease=null) {
			await v.FadeTo(0, time, ease);
			v.IsVisible = false;
		}

		/*public async Task<string> wrap_callback(Func<string, Task<string>> function, Func<string, Task<string>> callback, string parameter="") {
			await function(parameter);
			await callback(parameter);
			return "";
		}*/

		/*public async Task<bool> timeout(int milliseconds) {
			Task l_task = Task<int>.Factory.StartNew(() => this.async_timeout(milliseconds));
			await l_task;
			return true;
		}

		public int async_timeout(int milliseconds) {
			Thread.Sleep(milliseconds);
			return milliseconds;
		}*/

		public async Task<string> layout_to(View v, double x, double y, Rect rect, uint time, Easing easing, AbsoluteLayoutFlags flags=AbsoluteLayoutFlags.None, double width=-1, double height=-1) {
		/*	List<Element> children = this.children(v);
			View child = (View)children[0];
			double height = child.Height;
			double width = child.Width;*/
			if(width == -1 || height == -1) {
				width = v.Width;
				height = v.Height;
			}
            /*if(rect == null) {
                rect = new Rectangle(x, y, 1, 1);
            }*/
			Rect layout_bounds = new Rect(x, y, width, height);
			await v.LayoutTo(layout_bounds, time, easing);
			AbsoluteLayout.SetLayoutBounds(v, rect);
            //if(flags != null) {
                AbsoluteLayout.SetLayoutFlags(v, flags);
            //}
			return "";
		}

		/*public View dummy(Element e) {
			View copy = (View)this.copy(e);
			StackLayout dummy = this.FindByName<StackLayout>("dummy");
			dummy.Children.Clear();
			dummy.Children.Add(copy);
			return copy;
		}*/

		/*public async Task<dimensions> get_dimensions(Element e) {
			View v = this.dummy(e);
			await this.timeout(500);
			dimensions d = new dimensions(v.Width, v.Height);
			return d;
		}*/

		/*public void tapped(View v, EventHandler callback) {
			TapGestureRecognizer tap = new TapGestureRecognizer();
			tap.Tapped += callback;
			v.GestureRecognizers.Add(tap);
		}*/

        //Dictionary<string, dimensions> dimensions;

        /*public void dimensions(View e) {
            e.SizeChanged += (s, a) => {
                this.add_element_data(e, "width", e.Width.ToString());
                this.add_element_data(e, "height", e.Height.ToString());
            };
        }*/

        /*public Color from_rgb(string rgb) {
            List<string> split = new List<string>(rgb.Split(','));
            if(split[1].IndexOf("%") != -1) {
                split[0] = (Convert.ToDouble(split[0])/360).ToString();
                split[1] = (Convert.ToDouble(split[1].Substring(0, split[1].Length-1))/100).ToString();
                split[2] = (Convert.ToDouble(split[2].Substring(0, split[1].Length-1))/100).ToString();
            }
            Color rgb_color = Color.FromHsla(Convert.ToDouble(split[0]), Convert.ToDouble(split[1]), Convert.ToDouble(split[2]), 1);
            return rgb_color;
            //return Color.Red;
        }*/

        public string value(Element e) {
            string value = "";
            string type = e.ToString();
            switch(type) {
                case "Microsoft.Maui.Controls.Entry":
                    Entry entry = (Entry)e;
                    value = entry.Text;
                    break;
                case "Microsoft.Maui.Controls.Label":
                    Label label = (Label)e;
                    value = label.Text;
                    break;
                case "Microsoft.Maui.Controls.Picker":
                    Picker picker = (Picker)e;
                    object selected_object = picker.SelectedItem;
                    foreach(PropertyInfo property in selected_object.GetType().GetProperties()) {
                        object obj = property.GetValue(selected_object);
						if(property.Name == "id") {
                            value = obj.ToString();
						}
                    }
                    if(value == "") {
                        value = selected_object.ToString();
                    }
                    break;
                case "Microsoft.Maui.Controls.Editor":
                    Editor editor = (Editor)e;
                    value = editor.Text;
                    break;
            }
            //System.Diagnostics.Debug.WriteLine(type+" value: "+value);
            return value;
        }

        public Dictionary<string, string> values(List<Element> elements, string exclude_class=null) {
            Dictionary<string, string> values = new Dictionary<string, string>();
            foreach(Element e in elements) {
                if(exclude_class != null && this.has_class(e, exclude_class)) {

                } else {
                    string id = this.id(e);
                    string value = this.value(e);
                    values.Add(id, value);
                }
            }
            return values;
        }

        public string id(Element e, string set_id=null) {
            if(set_id != null) {
                this.edit_element_data(e, "id", set_id);
            }
            if(this.element_data(e).ContainsKey("id")) {
                return this.element_data(e)["id"];
            }
            return "";
        }

        public double absolute_y(Element e) {
            //System.Diagnostics.Debug.WriteLine(e);
            View v = (View)e;
            if(v.Parent == null) {
                return v.Y;
            }
            string element_type = e.Parent.ToString().Split('.').Reverse().ToList<string>()[0];
            if(element_type.ToLower().IndexOf("page") == -1) {
                return v.Y;
            }
            return v.Y+this.absolute_y(v.Parent);
        }
        
        public double absolute_x(Element e) {
            View v = (View)e;
            if(v.Parent == null) {
                return v.X;
            }
            string element_type = e.Parent.ToString().Split('.').Reverse().ToList<string>()[0];
            if(element_type.ToLower().IndexOf("page") == -1) {
                return v.X;
            }
            return v.X+this.absolute_x(v.Parent);
        }

        public double height_children(Element e) {
            List<Element> children = this.children(e);
            double height = 0;
            foreach(Element child in children) {
                View v = (View)child;
                height += v.Height;
            }
            return height;
        }
        
        public void tapped(View v, EventHandler callback, int tap_count=1, bool cancel_color=false) {
            Color set_color = Color.FromRgb(0, 168, 255);
			TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.NumberOfTapsRequired = tap_count;
            tap.Tapped += async (s, e) => {
                /*string icon_font = "icofont";
                Color store = Color.Blue;
                StackLayout layout = null;
                Label label = null;
                List<Element> labels = new List<Element>();
                List<Color> store_colors = new List<Color>();
                //v.Opacity = 0.5;
                if(v.ToString().IndexOf("Microsoft.Maui.Controls.Label") != -1) {
                    label = (Label)v;
                    store = label.TextColor;
                    //Color store = label.BackgroundColor;
                    label.TextColor = set_color;//Color.FromRgba(0, 168, 255, 1);
                    label.TextDecorations = TextDecorations.Underline;
                    //await Task.Delay(995);
                    //label.TextColor = store;
                    
                } else {
                    labels = this.find(v, "Label");
                    foreach(Label l in labels) {
                        store_colors.Add(l.TextColor);
                        l.TextColor = set_color;
                        if(!l.FontFamily.Contains(icon_font)) {
                            l.TextDecorations = TextDecorations.Underline;
                        }
                    }
                }
                if(v.StyleClass == null) {
                    v.StyleClass = new List<string>();
                }
                v.StyleClass.Add("active");
                await Task.Delay(50);
                //v.Opacity = 1;
                v.StyleClass.Remove("active");
                if(v.ToString() == "Microsoft.Maui.Controls.Label") {
                    label.TextDecorations = TextDecorations.None;
                    label.TextColor = store;
                } else {
                    int counter = 0;
                    //layout.BackgroundColor = store;
                    foreach(Label l in labels) {
                        Color color_value = store_colors[counter];
                        l.TextColor = color_value;
                        if(!l.FontFamily.Contains(icon_font)) {
                            l.TextDecorations = TextDecorations.None;
                        }
                        counter++;
                    }
                }*/
                if(!cancel_color) {
                    Color current_back = v.BackgroundColor;
                    v.BackgroundColor = Color.FromRgba(0,0,0,0.3);
                    callback(s, e);
                    await Task.Delay(50);
                    v.BackgroundColor = current_back;
                } else {
                    callback(s, e);
                }
                //
            };
			v.GestureRecognizers.Add(tap);
		}

    }

	public class option_value {
		public string option;
		public string value;
	}

	public class dimensions {
		public double width;
		public double height;

		public dimensions(double width, double height) {
			this.width = width;
			this.height = height;
		}
	}

	public class selector_split {
		public string prefix_command = " ";
		public string selector;
		public string next_command = " ";

		public selector_split(string next_command, string selector) {
			this.next_command = next_command;
			this.selector = selector;
		}
	}

    public class selector_pair {
        public string type_sel;
        public string class_sel;

        public selector_pair(string type_sel, string class_sel) {
            this.type_sel = type_sel;
            this.class_sel = class_sel;
        }
    }
}
