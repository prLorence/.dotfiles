exports.default=function(e){var t={};function n(o){if(t[o])return t[o].exports;var r=t[o]={i:o,l:!1,exports:{}};return e[o].call(r.exports,r,r.exports,n),r.l=!0,r.exports}return n.m=e,n.c=t,n.d=function(e,t,o){n.o(e,t)||Object.defineProperty(e,t,{enumerable:!0,get:o})},n.r=function(e){"undefined"!=typeof Symbol&&Symbol.toStringTag&&Object.defineProperty(e,Symbol.toStringTag,{value:"Module"}),Object.defineProperty(e,"__esModule",{value:!0})},n.t=function(e,t){if(1&t&&(e=n(e)),8&t)return e;if(4&t&&"object"==typeof e&&e&&e.__esModule)return e;var o=Object.create(null);if(n.r(o),Object.defineProperty(o,"default",{enumerable:!0,value:e}),2&t&&"string"!=typeof e)for(var r in e)n.d(o,r,function(t){return e[t]}.bind(null,r));return o},n.n=function(e){var t=e&&e.__esModule?function(){return e.default}:function(){return e};return n.d(t,"a",t),t},n.o=function(e,t){return Object.prototype.hasOwnProperty.call(e,t)},n.p="",n(n.s=0)}([function(e,t,n){"use strict";Object.defineProperty(t,"__esModule",{value:!0});const o=n(1);t.default=function(e){return{plugin:function(t,n){e.pluginId;o.quoteRenderer(t,n)},assets:function(){return[{name:"quoteRender.css"}]}}}},function(e,t,n){"use strict";Object.defineProperty(t,"__esModule",{value:!0}),t.quoteRenderer=void 0;const o=/\[color=(.*?)\]/,r=/\[name=(.*?)\]/,c=/\[date=(.*?)\]/;t.quoteRenderer=function(e,t){const n=e.renderer.rules.blockquote_open||function(e,t,n,o,r){return r.renderToken(e,t,n,o,r)};e.renderer.rules.blockquote_open=function(e,t,u,l,s){const a=e[t];let i=null,f=null;for(let n=t+1;n<e.length&&"blockquote_close"!==e[n].type;n++){const t=o.exec(e[n].content),u=r.exec(e[n].content),l=c.exec(e[n].content);if(t||u||l)for(const t of e[n].children){if("text"!==t.type)continue;let e=o.exec(t.content);e&&(t.content=t.content.replace(o,""),a.attrs||(a.attrs=[]),a.attrs.push(["style","border-color:"+e[1]]));let n=r.exec(t.content);n&&(t.content=t.content.replace(r,""),i=n[1]);let u=c.exec(t.content);u&&(t.content=t.content.replace(c,""),f=u[1])}}let d=n(e,t,u,l,s),p="";return i&&(p+=`<span class="blockquote-name blockquote-enhancement"><i class="fas fa-user"></i>${i}</span>`),f&&(p+=`<span class="blockquote-date blockquote-enhancement"><i class="fas fa-clock-o"></i>${f}</span>`),p.length>0&&(d+=`<small class="blockquote-enhancement-wrap">${p}</small>`),d}}}]).default;