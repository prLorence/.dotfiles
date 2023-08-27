!function(t){var e={};function n(o){if(e[o])return e[o].exports;var i=e[o]={i:o,l:!1,exports:{}};return t[o].call(i.exports,i,i.exports,n),i.l=!0,i.exports}n.m=t,n.c=e,n.d=function(t,e,o){n.o(t,e)||Object.defineProperty(t,e,{enumerable:!0,get:o})},n.r=function(t){"undefined"!=typeof Symbol&&Symbol.toStringTag&&Object.defineProperty(t,Symbol.toStringTag,{value:"Module"}),Object.defineProperty(t,"__esModule",{value:!0})},n.t=function(t,e){if(1&e&&(t=n(t)),8&e)return t;if(4&e&&"object"==typeof t&&t&&t.__esModule)return t;var o=Object.create(null);if(n.r(o),Object.defineProperty(o,"default",{enumerable:!0,value:t}),2&e&&"string"!=typeof t)for(var i in t)n.d(o,i,function(e){return t[e]}.bind(null,i));return o},n.n=function(t){var e=t&&t.__esModule?function(){return t.default}:function(){return t};return n.d(e,"a",e),e},n.o=function(t,e){return Object.prototype.hasOwnProperty.call(t,e)},n.p="",n(n.s=2)}([function(t,e,n){"use strict";Object.defineProperty(e,"__esModule",{value:!0}),e.default=joplin},function(t,e,n){"use strict";var o=this&&this.__awaiter||function(t,e,n,o){return new(n||(n=Promise))((function(i,a){function r(t){try{u(o.next(t))}catch(t){a(t)}}function l(t){try{u(o.throw(t))}catch(t){a(t)}}function u(t){var e;t.done?i(t.value):(e=t.value,e instanceof n?e:new n((function(t){t(e)}))).then(r,l)}u((o=o.apply(t,e||[])).next())}))};Object.defineProperty(e,"__esModule",{value:!0}),e.DataApi=void 0;const i=n(0),a=n(5),r=n(6),l=n(7);class u{constructor(t){this.allTags=t,this.tag=new a.TagAPI(this),this.note=new r.NoteAPI(this),this.notebook=new l.NotebookAPI(this)}static builder(){return o(this,void 0,void 0,(function*(){const t=yield u.getAll(["tags"],{fields:["id","title"]});return new u(t)}))}addParentTags(t,e){return o(this,void 0,void 0,(function*(){const n=yield u.getAll(["tags",t.id,"notes"],{fields:["id"]});for(let t of n)console.log(`adding tag ${e.title} with id ${e.id} to note ${t.id}`),yield u.setNoteTag(t.id,e.id)}))}getTagsByName(t){return this.allTags.filter(e=>e.title===t)}getNotesWithTag(t){return o(this,void 0,void 0,(function*(){return yield u.getAll(["tags",t,"notes"],{fields:["id","title","body"]})}))}getNoteTags(t){return o(this,void 0,void 0,(function*(){return yield u.getAll(["notes",t.id,"tags"],{fields:["id","title"]})}))}createTag(t){return o(this,void 0,void 0,(function*(){return yield i.default.data.post(["tags"],null,{title:t})}))}getTaggedNotesUnderFolder(t,e){return o(this,void 0,void 0,(function*(){return yield u.getAll(["search"],{query:`tag:"${t}" -notebook:"${e}"`,type:"note"})}))}static setNoteTag(t,e){return o(this,void 0,void 0,(function*(){yield i.default.data.post(["tags",e,"notes"],null,{id:t})}))}static getAll(t,e){return o(this,void 0,void 0,(function*(){let n=[],o=!0,a=1;for(;o&&a<1e3;){let r=yield i.default.data.get(t,Object.assign(Object.assign({},e),{page:a}));o=r.has_more,n=n.concat(r.items),a+=1}return n}))}}e.DataApi=u},function(t,e,n){"use strict";var o=this&&this.__awaiter||function(t,e,n,o){return new(n||(n=Promise))((function(i,a){function r(t){try{u(o.next(t))}catch(t){a(t)}}function l(t){try{u(o.throw(t))}catch(t){a(t)}}function u(t){var e;t.done?i(t.value):(e=t.value,e instanceof n?e:new n((function(t){t(e)}))).then(r,l)}u((o=o.apply(t,e||[])).next())}))};Object.defineProperty(e,"__esModule",{value:!0});const i=n(0),a=n(3),r=n(4),l=n(1),u=n(8),c=n(9);i.default.plugins.register({onStart:function(){return o(this,void 0,void 0,(function*(){yield i.default.settings.registerSection("tagRuleSection",{label:"Tag Rule Settings",iconName:"fas fa-tag"}),yield i.default.settings.registerSettings({[r.PARENT_TAG_RELATION_SETTING]:{value:r.PARENT_TAG_RELATION_SETTING,type:a.SettingItemType.String,section:"tagRuleSection",public:!0,label:"Common tag for all notes specifying parent tags"}}),yield i.default.settings.registerSettings({[c.MOVE_TAG_RULE_TAG_SETTING_NAME]:{value:c.MOVE_TAG_RULE_TAG_SETTING_NAME,type:a.SettingItemType.String,section:"tagRuleSection",public:!0,label:"Common tag for all notes specifying move note rules"}}),yield i.default.commands.register({name:"updateTags",label:"Update tags based on user defined rules",execute:()=>o(this,void 0,void 0,(function*(){console.info("UPDATING TAGS");const t=yield l.DataApi.builder(),e=yield r.getTagParentRelationships(t);console.log("tagParents.length "+e.length),yield r.addParentTagsToChildren(t,e)}))}),yield i.default.commands.register({name:"deleteDuplicateTags",label:"Delete duplicate tags",execute:()=>o(this,void 0,void 0,(function*(){console.info("DELETING DUPLICATE TAGS");const t=yield l.DataApi.builder();yield u.deduplicateTags(t)}))}),yield i.default.commands.register({name:"moveTaggedNotes",label:"Move tagged notes based on custom rules",execute:()=>o(this,void 0,void 0,(function*(){console.info("MOVING TAGGED NOTES");const t=yield l.DataApi.builder();yield c.moveTaggedNotes(t)}))})}))}})},function(t,e,n){"use strict";var o;Object.defineProperty(e,"__esModule",{value:!0}),e.ContentScriptType=e.SettingStorage=e.AppType=e.SettingItemSubType=e.SettingItemType=e.ToolbarButtonLocation=e.isContextMenuItemLocation=e.MenuItemLocation=e.ModelType=e.ImportModuleOutputFormat=e.FileSystemItem=void 0,function(t){t.File="file",t.Directory="directory"}(e.FileSystemItem||(e.FileSystemItem={})),function(t){t.Markdown="md",t.Html="html"}(e.ImportModuleOutputFormat||(e.ImportModuleOutputFormat={})),function(t){t[t.Note=1]="Note",t[t.Folder=2]="Folder",t[t.Setting=3]="Setting",t[t.Resource=4]="Resource",t[t.Tag=5]="Tag",t[t.NoteTag=6]="NoteTag",t[t.Search=7]="Search",t[t.Alarm=8]="Alarm",t[t.MasterKey=9]="MasterKey",t[t.ItemChange=10]="ItemChange",t[t.NoteResource=11]="NoteResource",t[t.ResourceLocalState=12]="ResourceLocalState",t[t.Revision=13]="Revision",t[t.Migration=14]="Migration",t[t.SmartFilter=15]="SmartFilter",t[t.Command=16]="Command"}(e.ModelType||(e.ModelType={})),function(t){t.File="file",t.Edit="edit",t.View="view",t.Note="note",t.Tools="tools",t.Help="help",t.Context="context",t.NoteListContextMenu="noteListContextMenu",t.EditorContextMenu="editorContextMenu",t.FolderContextMenu="folderContextMenu",t.TagContextMenu="tagContextMenu"}(o=e.MenuItemLocation||(e.MenuItemLocation={})),e.isContextMenuItemLocation=function(t){return[o.Context,o.NoteListContextMenu,o.EditorContextMenu,o.FolderContextMenu,o.TagContextMenu].includes(t)},function(t){t.NoteToolbar="noteToolbar",t.EditorToolbar="editorToolbar"}(e.ToolbarButtonLocation||(e.ToolbarButtonLocation={})),function(t){t[t.Int=1]="Int",t[t.String=2]="String",t[t.Bool=3]="Bool",t[t.Array=4]="Array",t[t.Object=5]="Object",t[t.Button=6]="Button"}(e.SettingItemType||(e.SettingItemType={})),function(t){t.FilePathAndArgs="file_path_and_args",t.FilePath="file_path",t.DirectoryPath="directory_path"}(e.SettingItemSubType||(e.SettingItemSubType={})),function(t){t.Desktop="desktop",t.Mobile="mobile",t.Cli="cli"}(e.AppType||(e.AppType={})),function(t){t[t.Database=1]="Database",t[t.File=2]="File"}(e.SettingStorage||(e.SettingStorage={})),function(t){t.MarkdownItPlugin="markdownItPlugin",t.CodeMirrorPlugin="codeMirrorPlugin"}(e.ContentScriptType||(e.ContentScriptType={}))},function(t,e,n){"use strict";var o=this&&this.__awaiter||function(t,e,n,o){return new(n||(n=Promise))((function(i,a){function r(t){try{u(o.next(t))}catch(t){a(t)}}function l(t){try{u(o.throw(t))}catch(t){a(t)}}function u(t){var e;t.done?i(t.value):(e=t.value,e instanceof n?e:new n((function(t){t(e)}))).then(r,l)}u((o=o.apply(t,e||[])).next())}))};Object.defineProperty(e,"__esModule",{value:!0}),e.addParentTagsToChildren=e.getTagParentRelationships=e.PARENT_TAG_RELATION_SETTING=void 0;const i=n(0);e.PARENT_TAG_RELATION_SETTING="joplin-parent-tag-relation",e.getTagParentRelationships=function(t){return o(this,void 0,void 0,(function*(){const n=yield function(){return o(this,void 0,void 0,(function*(){return yield i.default.settings.value(e.PARENT_TAG_RELATION_SETTING)}))}(),a=t.getTagsByName(n);if(0===a.length)return console.warn("Cannot find tag "+n),console.log(t.allTags),[];let r=[];for(let e of a)r=r.concat(yield t.getNotesWithTag(e.id));let l=[];for(let t of r){const e=JSON.parse(t.body),n=Object.keys(e).map(t=>({childTagName:t,parentTagName:e[t]}));l=l.concat(n)}return l}))},e.addParentTagsToChildren=function(t,e){return o(this,void 0,void 0,(function*(){for(let n of e){const e=t.getTagsByName(n.childTagName);let i=t.getTagsByName(n.parentTagName);if(0!==e.length){if(i.length>1){let e=i.filter(e=>o(this,void 0,void 0,(function*(){return(yield t.getNotesWithTag(e.id)).length>0})));i=0===e.length?[i[0]]:e}0===i.length&&(console.warn("Creating new tag "+n.parentTagName),i.push(yield t.createTag(n.parentTagName)));for(let n of e)for(let e of i)yield t.addParentTags(n,e)}else console.warn("Unable to find tag "+n.childTagName)}}))}},function(t,e,n){"use strict";var o=this&&this.__awaiter||function(t,e,n,o){return new(n||(n=Promise))((function(i,a){function r(t){try{u(o.next(t))}catch(t){a(t)}}function l(t){try{u(o.throw(t))}catch(t){a(t)}}function u(t){var e;t.done?i(t.value):(e=t.value,e instanceof n?e:new n((function(t){t(e)}))).then(r,l)}u((o=o.apply(t,e||[])).next())}))};Object.defineProperty(e,"__esModule",{value:!0}),e.TagAPI=void 0;const i=n(0),a=n(1);e.TagAPI=class{constructor(t){this.dataApi=t}delete(t){return o(this,void 0,void 0,(function*(){yield i.default.data.delete(["tags",t.id])}))}getByName(t){return o(this,void 0,void 0,(function*(){return yield a.DataApi.getAll(["search"],{query:t,type:"tag"})}))}}},function(t,e,n){"use strict";var o=this&&this.__awaiter||function(t,e,n,o){return new(n||(n=Promise))((function(i,a){function r(t){try{u(o.next(t))}catch(t){a(t)}}function l(t){try{u(o.throw(t))}catch(t){a(t)}}function u(t){var e;t.done?i(t.value):(e=t.value,e instanceof n?e:new n((function(t){t(e)}))).then(r,l)}u((o=o.apply(t,e||[])).next())}))};Object.defineProperty(e,"__esModule",{value:!0}),e.NoteAPI=void 0;const i=n(0),a=n(1);class r{constructor(t){this.dataApi=t}getByTagName(t){return o(this,void 0,void 0,(function*(){return yield a.DataApi.getAll(["search"],{query:`tag:"${t}"`,type:"note",fields:r.FIELDS})}))}getByTagNameUnderFolder(t,e){return o(this,void 0,void 0,(function*(){return yield a.DataApi.getAll(["search"],{query:`tag:"${t}" notebook:"${e}"`,type:"note",fields:r.FIELDS})}))}getByTagNameExcludeFolder(t,e){return o(this,void 0,void 0,(function*(){return yield a.DataApi.getAll(["search"],{query:`tag:"${t}" -notebook:"${e}"`,type:"note",fields:r.FIELDS})}))}moveNoteToFolder(t,e){return o(this,void 0,void 0,(function*(){const n=yield this.dataApi.notebook.getByName(e);yield i.default.data.put(["notes",t.id],null,{parent_id:n.id})}))}}e.NoteAPI=r,r.FIELDS=["id","title","body"]},function(t,e,n){"use strict";var o=this&&this.__awaiter||function(t,e,n,o){return new(n||(n=Promise))((function(i,a){function r(t){try{u(o.next(t))}catch(t){a(t)}}function l(t){try{u(o.throw(t))}catch(t){a(t)}}function u(t){var e;t.done?i(t.value):(e=t.value,e instanceof n?e:new n((function(t){t(e)}))).then(r,l)}u((o=o.apply(t,e||[])).next())}))};Object.defineProperty(e,"__esModule",{value:!0}),e.NotebookAPI=void 0;const i=n(1);class a{constructor(t){this.dataApi=t}getByName(t){return o(this,void 0,void 0,(function*(){const e=yield i.DataApi.getAll(["search"],{query:""+t,type:"folder",fields:a.FIELDS});if(1!==e.length)throw new Error(`Notebook name ${t} is note unique to one notebook`);return e[0]}))}}e.NotebookAPI=a,a.FIELDS=["id","title","parent_id"]},function(t,e,n){"use strict";var o=this&&this.__awaiter||function(t,e,n,o){return new(n||(n=Promise))((function(i,a){function r(t){try{u(o.next(t))}catch(t){a(t)}}function l(t){try{u(o.throw(t))}catch(t){a(t)}}function u(t){var e;t.done?i(t.value):(e=t.value,e instanceof n?e:new n((function(t){t(e)}))).then(r,l)}u((o=o.apply(t,e||[])).next())}))};Object.defineProperty(e,"__esModule",{value:!0}),e.deduplicateTags=void 0,e.deduplicateTags=function(t){return o(this,void 0,void 0,(function*(){const e=function(t){const e=t.reduce((t,e)=>(t[e.title]=t[e.title]||[],t[e.title].push(e),t),{});return Object.values(e).filter(t=>t.length>1)}(t.allTags);if(0!==e.length){console.warn("Found duplicates"),console.log(e);for(let n of e){const e=n.pop();if(e)for(let o of n)yield t.addParentTags(o,e),yield t.tag.delete(o);else console.warn("Something has gone wrong. Skipping tag")}}}))}},function(t,e,n){"use strict";var o=this&&this.__awaiter||function(t,e,n,o){return new(n||(n=Promise))((function(i,a){function r(t){try{u(o.next(t))}catch(t){a(t)}}function l(t){try{u(o.throw(t))}catch(t){a(t)}}function u(t){var e;t.done?i(t.value):(e=t.value,e instanceof n?e:new n((function(t){t(e)}))).then(r,l)}u((o=o.apply(t,e||[])).next())}))};Object.defineProperty(e,"__esModule",{value:!0}),e.moveTaggedNotes=e.MOVE_TAG_RULE_TAG_SETTING_NAME=void 0;const i=n(10);function a(t){return"string"==typeof t.tag&&("string"==typeof t.toNotebook&&(!t.fromNotebook||"string"==typeof t.fromNotebook))}function r(t,e){return o(this,void 0,void 0,(function*(){let n;console.log(t),n=t.fromNotebook?yield e.note.getByTagNameUnderFolder(t.tag,t.fromNotebook):yield e.note.getByTagNameExcludeFolder(t.tag,t.toNotebook);for(let o of n)yield e.note.moveNoteToFolder(o,t.toNotebook)}))}e.MOVE_TAG_RULE_TAG_SETTING_NAME="joplin-move-tag-rule",e.moveTaggedNotes=function(t){return o(this,void 0,void 0,(function*(){const n=yield function(t){return o(this,void 0,void 0,(function*(){const n=yield i.settings.getSetting(e.MOVE_TAG_RULE_TAG_SETTING_NAME);return(yield t.note.getByTagName(n)).map(t=>JSON.parse(t.body)).filter(a)}))}(t);for(let e of n)yield r(e,t)}))}},function(t,e,n){"use strict";var o=this&&this.__awaiter||function(t,e,n,o){return new(n||(n=Promise))((function(i,a){function r(t){try{u(o.next(t))}catch(t){a(t)}}function l(t){try{u(o.throw(t))}catch(t){a(t)}}function u(t){var e;t.done?i(t.value):(e=t.value,e instanceof n?e:new n((function(t){t(e)}))).then(r,l)}u((o=o.apply(t,e||[])).next())}))};Object.defineProperty(e,"__esModule",{value:!0}),e.settings=void 0;const i=n(0);e.settings={getSetting:t=>o(void 0,void 0,void 0,(function*(){return yield i.default.settings.value(t)}))}}]);