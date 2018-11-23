var UserRoleSelector = (function () {
    var UserRoleSelector = function (options) {
        this.init(options || {});
        this.bind();
    }

    var h = '<div class="row"><div class="data-bar"><a href="#" class="close button">关闭</a><a href="#" class="save button">保存</a></div>'
        +'<div class="col-lg-6"><div class="input-group">'
	    + '<input type="text" class="form-control">'
	    + '<span class="input-group-btn">'
	    + '	<button class="btn btn-default" type="button">Go</button>'
	    + '</span>'
	    + '</div></div></div>';

    var html = '<div class="user-role-selector">' + 
        '<ul class="left-list data-list">' +
        '</ul>' +
        '<div class="data-oper">' +
        '<a href="#" class="add button">添加</a>' +
        '<a href="#" class="del button">删除</a>' +
        '</div>' +
        '<ul id="right-list" class="right-list data-list"> </ul>' +
        '<div class="data-bar"><a href="#" class="close button">关闭</a>' +
        '<a href="#" class="save button">保存</a></div> ' +
        '</div>';

    UserRoleSelector.prototype = {
        init: function (options) {
            this.options = options;
            this.dom = document.createElement("div");
            this.dom.className = "mask";
            this.dom.style.display = this.options.show ? "block" : "none";
            this.dom.innerHTML = html;
            this.status = this.options.show ? 1 : 0;
            document.body.appendChild(this.dom);

            this.left = this.dom.querySelector(".left-list.data-list");
            this.right = this.dom.querySelector(".right-list.data-list");
            this.add = this.dom.querySelector(".button.add");
            this.del = this.dom.querySelector(".button.del");
            this.save = this.dom.querySelector(".button.save");
            this.close = this.dom.querySelector(".button.close");

            var data = this.options.data || [];
            for (var i = 0; i < data.length; i++) {
                this.left.innerHTML += "<li data-value=" + data[i].value + ">" + data[i].text + "</li>";
            }

            var rightdata = this.options.rightdata || [];
            for (var i = 0; i < rightdata.length; i++) {
                this.right.innerHTML += "<li data-value=" + rightdata[i].value + ">" + rightdata[i].text + "</li>";
            }

            this.items = this.left.querySelectorAll("li");
            this.rightitems = this.right.querySelectorAll("li");
        },
 
        bind: function () {
            var _this = this;
            if (this.options.onsave) {
               
                this.save.onclick = _this.options.onsave.bind(_this);
            }
            
            this.close.onclick = function () {
                _this.hide();
            }

            this.add.onclick = this._operClick.bind(this, this.add);
            this.del.onclick = this._operClick.bind(this, this.del);
          
            for (var i = 0; i < this.items.length; i++) {
                this.items[i].onclick = this._itemClick;
            }

            for (var i = 0; i < this.rightitems.length; i++) {
                this.rightitems[i].onclick = this._itemClick;
            }
        },
        show: function () {
            this.dom.style.display = "block";
            this.status = 1;
        },
        hide: function () {
            this.dom.style.display = "none";
            this.status = 0;
        },
        getValues: function () {
            var values = "";
            var selecteds = this.right.querySelectorAll("li");
            for (var i = 0; i < selecteds.length; i++) {
                values += selecteds[i].getAttribute("data-value");
                if (i != selecteds.length - 1)
                    values += ",";
            }
            return values;
        },
        _operClick: function (target) {
            var one, two;
            if (target.className.indexOf("add") != -1) {
                one = this.left;
                two = this.right;
            }
            else {
                one = this.right;
                two = this.left;
            }
            var selecteds = one.querySelectorAll("li.selected");
            for (var i = 0; i < selecteds.length; i++) {
                two.appendChild(selecteds[i]);
            }
        },
        _itemClick: function () {
            if (this.className.indexOf("selected") != -1) {
                this.className = "";
            }
            else {
                this.className = "selected";
            }
        },
    };
    return UserRoleSelector;
})();