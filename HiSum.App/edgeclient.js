

function displaySentencesDiv() {
    $("#sentencesDiv").show();
    $("#usersDiv").hide();
    $("#keywordsDiv").hide();
    $("#tagCloudDiv").hide();
    $("#btnUp").show();
    $("#btnFollowUser").show();
    $("#btnWatchKeyword").show();
    $("#divTime").show();
}

function displayUsersDiv() {
    $("#sentencesDiv").hide();
    $("#usersDiv").show();
    $("#keywordsDiv").hide();
    $("#tagCloudDiv").hide();
}

function displayKeywordsDiv() {
    $("#sentencesDiv").hide();
    $("#usersDiv").hide();
    $("#keywordsDiv").show();
    $("#tagCloudDiv").hide();
}

function displayTagCloudDiv() {
    $("#sentencesDiv").hide();
    $("#usersDiv").hide();
    $("#keywordsDiv").hide();
    $("#tagCloudDiv").show();
}

function up() {
    var currRootNode = $("#treeDiv").fancytree("getTree").getRootNode().getFirstChild();
    var currRootNodeKey = currRootNode.key;
    var rootNodeHidden = $("#hiddenTreeDiv").fancytree("getTree").getNodeByKey(currRootNodeKey);
    var parentNode = rootNodeHidden.getParent();
    var parentKey = parentNode.key;
    loadTreeByKey(parentKey);
}

function loadTreeByKey(key) {
    var dict = $("#hiddenTreeDiv").fancytree("getTree").getNodeByKey(key).toDict(true);
    $("#treeDiv").fancytree("getTree").getRootNode().removeChildren();
    $("#treeDiv").fancytree("getTree").getRootNode().addNode(dict);
    expandFullTree();
}

function enableLinks() {
    // Load native UI library
    var nw = require('nw.gui');

    // Create an empty menu
    var menu = new nw.Menu();
    // Add an item with label
    menu.append(new nw.MenuItem({
        label: 'open in browser',
        click: function (e) {
            nw.Shell.openExternal('http://google.com');
        }
    }));
    // Listen for a right click on link
    var elm = document.getElementsByTagName('a');
    for (var i = 0; i < elm.length; i++) {
        elm[i].addEventListener('contextmenu', function (e) {
            e.preventDefault();
            menu.popup(e.x, e.y);
        });
    }
}

function fetchFrontPage(pageNum) {
    try {
        pageNum = typeof pageNum !== 'undefined' ? pageNum : 1;
        var stories = [];
        var allUsersInFrontPage = [];
        var allUsersFollowing = [];
        var allKeywordsInFrontPage = [];
        var allKeywordsWatching = [];
        getFollowingFunction(pageNum, function (error, result) {
            $.each(result, function (key, value) {
                allUsersFollowing.push(value);
            });
        });
        getWatchingFunction(pageNum, function (error, result) {
            $.each(result, function (key, value) {
                allKeywordsWatching.push(value);
            });
        });
        top100Function(pageNum, function (error, result) {
            try {
                if (error) console.log(error);
                result.forEach(function (entry) {
                    if (!(entry['Url']) || entry['Url'] === '') {
                        entry['Url'] = entry['CommentUrl'];
                    }
                    var story = { author: entry['Author'], title: entry['StoryTitle'], text: entry['StoryText'] ? entry['StoryText'].substring(1, 100) : '', storyid: entry['StoryId'], storyurl: entry['Url'], hdnstoryid: 'hdn' + entry['StoryId'], btnDeleteStoryId: 'btnDelete_' + entry['StoryId'], count: entry['StoryComments'], commentUrl: entry['CommentUrl'], storyurlwithquotes: '\'' + entry['Url'] + '\'', commenturlwithquotes: '\'' + entry['CommentUrl'] + '\'', allUserComments: entry['AllUserComments'], archiveButtonId: 'archive_' + entry['StoryId'] };
                    $.each(entry["AllUserComments"], function (key, value) {
                        allUsersInFrontPage.push(value);
                    });
                    $.each(entry["AllKeywordComments"], function (key, value) {
                        allKeywordsInFrontPage.push(value);
                    });
                    stories.push(story);
                });
            } catch (ex) {
                alert(ex.toString());
            }
        });
        showFollowingOnFrontPage(allUsersInFrontPage, allUsersFollowing);
        showWatchingOnFrontPage(allKeywordsInFrontPage, allKeywordsWatching);
        render(stories);
    } catch (ex) {
        console.log(ex);
    }
}

function findIndex(object,value) {
    var i = 0;
    for (i = 0; i < object.length; i++) {
        if (object[i].user === value) {
            return i;
        }
    }
    return 0;
}

function updateData(object, keyValue, dataKeyValue) {
    var indexVal = findIndex(object, keyValue);
    if (indexVal === 0) {
        object.push({ 'user': keyValue, 'idlist': dataKeyValue });
    } else {
        object[indexVal].idlist = dataKeyValue;
    }
}

function showWatchingOnFrontPage(allKeywordsInFrontPage, allKeywordsWatching) {
    var uniqueKeywordsInFrontPage = [];
    var htmlWatching = '';
    $.each(allKeywordsInFrontPage, function (key, value) {
        var storyIdArray = [];
        var existingArray = [];
        existingArray = _.map(
            _.where(uniqueKeywordsInFrontPage, { user: value['Keyword'] }),
            function (item) {
                return { idlist: item.idlist };
            }
            );
        if (existingArray.length > 0) {
            storyIdArray.push.apply(storyIdArray, existingArray[0].idlist);
        }
        storyIdArray.push(value['StoryId']);
        storyIdArray = _.uniq(storyIdArray);
        updateData(uniqueKeywordsInFrontPage, value['Keyword'], storyIdArray);
    });
    uniqueKeywordsInFrontPage = _.sortBy(uniqueKeywordsInFrontPage, function (obj) {
        return obj.idlist.length
    }).reverse();
    $.each(uniqueKeywordsInFrontPage, function (key, value) {
        if ($.inArray(value['user'], allKeywordsWatching) > -1) {
            var label = $('<label/>', {
                for: 'remember',
                class: 'pure-checkbox'
            });
            var inp = $('<input/>', {
                onclick: showStoriesForSelectedFilters(),
                type: 'checkbox',
                name: 'cbx_' + value['user'],
                value: value['idlist'].join(','),
                text: value['user'] + '(' + value['idlist'].length + ')'
            });
            var br = $('<br/>');
            htmlWatching += "<label for='remember' class='pure-checkbox'><input onclick='showStoriesForSelectedFilters();' type='checkbox' name='cbx_" + value['user'] + "' value='" + value['idlist'].join(',') + "'>" + value['user'] + '(' + value['idlist'].length + ')' + "</label><br/>";
            label.append(inp);
            label.append(br);
        }
    });
    $("#divWatching").html(htmlWatching);
}

function showFollowingOnFrontPage(allUsersInFrontPage, allUsersFollowing) {
    var uniqueUsersInFrontPage = [];
    var htmlFollowing = '';
    $.each(allUsersInFrontPage, function (key, value) {
        var storyIdArray = [];
        var existingArray = [];
        existingArray = _.map(
            _.where(uniqueUsersInFrontPage, { user: value['User'] }),
            function (item) {
                return { idlist: item.idlist };
            }
            );
        if (existingArray.length > 0) {
            storyIdArray.push.apply(storyIdArray, existingArray[0].idlist);
        }
        storyIdArray.push(value['StoryId']);
        storyIdArray = _.uniq(storyIdArray);
        updateData(uniqueUsersInFrontPage, value['User'], storyIdArray);
    });
    uniqueUsersInFrontPage=_.sortBy(uniqueUsersInFrontPage, function (obj) {
        return obj.idlist.length
    }).reverse();
    $.each(uniqueUsersInFrontPage, function (key, value) {
        if ($.inArray(value['user'], allUsersFollowing) > -1) {
            var label = $('<label/>', {
                for: 'remember',
                class:'pure-checkbox'
            });
            var inp = $('<input/>', {
                onclick: showStoriesForSelectedFilters(),
                type: 'checkbox',
                name: 'cbx_' + value['user'],
                value: value['idlist'].join(','),
                text: value['user'] + '(' + value['idlist'].length + ')'
            });
            var br = $('<br/>');
            htmlFollowing += "<label for='remember' class='pure-checkbox'><input onclick='showStoriesForSelectedFilters();' type='checkbox' name='cbx_" + value['user'] + "' value='" + value['idlist'].join(',') + "'>" + value['user'] + '(' + value['idlist'].length + ')' + "</label><br/>";
            label.append(inp);
            label.append(br);
        }
    });
    $("#divFollowing").html(htmlFollowing);
}

function showStoriesForSelectedFilters() {
    var selectedKeywords = [];
    $("#divWatching input").each(function (key, itm) {
        if (itm.checked) {
            selectedKeywords.push(itm.value.split(','));
        }
    });
    var selectedUsers = [];
    $("#divFollowing input").each(function (key, itm) {
        if (itm.checked) {
            selectedUsers.push(itm.value.split(','));
        }
    });
    if (selectedKeywords.length === 0 && selectedUsers.length === 0) {
        //remove all filters from storyDiv
        $("#stories .email-item").show();
        $("#stories .email-item-selected").show();
    } else {
        //add filters to storyDiv
        $("#stories .email-item").hide();
        $("#stories .email-item-selected").hide();
        $.each(selectedKeywords, function (key, value) {
            $.each(value, function (key1, value1) { $('#' + value1).show(); });
        });
        $.each(selectedUsers, function (key, value) {
            $.each(value, function (key1, value1) { $('#' + value1).show(); });
        });
    }
}

function displayArchive() {
    var stories = [];
    getArchiveFunction(100, function (error, result) {
        try {
            if (error) console.log(error);
            result.forEach(function (entry) {
                var story = { author: entry['Author'], title: entry['StoryTitle'], text: entry['StoryText'] ? entry['StoryText'].substring(1, 100) : '', storyid: entry['StoryId'], storyurl: entry['Url'], hdnstoryid: 'hdn' + entry['StoryId'], btnDeleteStoryId: 'btnDelete_' + entry['StoryId'], count: entry['StoryComments'], commentUrl: entry['CommentUrl'], storyurlwithquotes: '\'' + entry['Url'] + '\'', commenturlwithquotes: '\'' + entry['CommentUrl'] + '\'', archiveButtonId: 'archive_' + entry['StoryId'] };
                stories.push(story);
            });
        } catch (ex) {
            alert(ex.toString());
        }
    });
    renderArchive(stories);
}

function renderArchive(stories) {
    $("#archiveStories").html(
        $("#archiveStoryTemplate").render(stories)
    );
    $("#stories").hide();
    $("#individualStories").hide();
    $("#archiveStories").show();
    $("#storyButtons").show();
}

function showStories() {
    $("#stories").show();
    $("#individualStories").show();
    $("#archiveStories").hide();
}

function render(stories) {
    $("#stories").html(
        $("#storyTemplate").render(stories)
    );
    $("#stories").show();
    $("#archiveStories").hide();
    $("#storyButtons").show();
}

function renderSingleStory(stories) {
    $("#individualStories").prepend(
        $("#individualStoryTemplate").render(stories)
    );
    $("#storyButtons").show();
}

function getSingleStory() {
    try {
        var stories = [];
        var storytext = $('#inputStoryId').val();
        var storyidtext = storytext.split('=')[1];
        var storyidval = parseInt(storyidtext);
        storyFunction(storyidval, function (error, result) {
            try {
                if (error) console.log(error);
                result.forEach(function (entry) {
                    if (!(entry['Url']) || entry['Url'] === '') {
                        entry['Url'] = entry['CommentUrl'];
                    }
                    var story = { author: entry['Author'], title: entry['StoryTitle'], text: entry['StoryText'] ? entry['StoryText'].substring(1, 100) : '', storyid: entry['StoryId'], storyurl: entry['Url'], hdnstoryid: 'hdn' + entry['StoryId'], btnDeleteStoryId: 'btnDelete_' + entry['StoryId'], count: entry['StoryComments'], commentUrl: entry['CommentUrl'], storyurlwithquotes: '\'' + entry['Url'] + '\'', commenturlwithquotes: '\'' + entry['CommentUrl'] + '\'', archiveButtonId: 'archive_' + entry['StoryId'] };
                    stories.push(story);
                });
            } catch (ex) {
                alert(ex.toString());
            }
        });
        renderSingleStory(stories);
        showStories();
    } catch (ex) {
        console.log(ex.toString());
    }
}

function loadStoryOnRefresh() {
    var storyidval = parseInt($("#hdnStoryId").html());
    loadStory(storyidval);
}

function watchKeywordToggle() {
    var keyword = $("#hdnKeyword").html();
    if ($("#btnWatchKeyword").text().indexOf("Unwatch") > -1) {
        unwatchKeywordFunction(keyword, function (error, result) {
            try {
                if (error) {
                    console.log(error);
                } else {
                    $("#btnWatchKeyword").text("Watch " + keyword);
                }
                //console.log(result);
            } catch (ex) {
                alert(ex.toString());
            }
        });
    } else {
        watchKeywordFunction(keyword, function (error, result) {
            try {
                if (error) {
                    console.log(error);
                } else {
                    $("#btnWatchKeyword").text("Unwatch " + keyword);
                }
                //console.log(result);
            } catch (ex) {
                alert(ex.toString());
            }
        });

    }
    refreshWatchingHiddenFieldList();
}

function followUserToggle() {
    var username = $("#hdnUser").html();
    if ($("#btnFollowUser").text().indexOf("Unfollow") > -1) {
        
        unfollowUserFunction(username, function (error, result) {
            try {
                if (error) {
                    console.log(error);
                } else {
                    $("#btnFollowUser").text("Follow " + username);
                }
                //console.log(result);
            } catch (ex) {
                alert(ex.toString());
            }
        });
    } else {
        followUserFunction(username, function (error, result) {
            try {
                if (error) {
                    console.log(error);
                } else {
                    $("#btnFollowUser").text("Unfollow " + username);
                }
                //console.log(result);
            } catch (ex) {
                alert(ex.toString());
            }
        });
    }
    refreshFollowingHiddenFieldList();
}

function unfollowUser() {
    var username = $("#hdnUser").html();
    unfollowUserFunction(username, function (error, result) {
        try {
            if (error) console.log(error);
            //console.log(result);
        } catch (ex) {
            alert(ex.toString());
        }
    });
}

function archiveStory(item) {
    var storyidval = parseInt(item.id.split('_')[1]);
    archive(storyidval);
    if (!e) var e = window.event;
    e.cancelBubble = true;
    if (e.stopPropagation) e.stopPropagation();
}

function getTreeFromJson(json) {
    // preserve newlines, etc - use valid JSON
    json = json.replace(/\\n/g, "\\n")
        .replace(/\\'/g, "\\'")
        .replace(/\\"/g, '\\"')
        .replace(/\\&/g, "\\&")
        .replace(/\\r/g, "\\r")
        .replace(/\\t/g, "\\t")
        .replace(/\\b/g, "\\b")
        .replace(/\\f/g, "\\f");
    // remove non-printable and other non-valid JSON chars
    json = json.replace(/[\u0000-\u0019]+/g, "");
    //console.log(json);
    var arr = JSON.parse(json);
    return arr;
}

function loadStory(storyidval) {
    try {
        fullStoryFunction(storyidval, function (error, result) {
            if (error) console.log('error:' + error);
            var json = result['Json'];
            var tagCloudJson = result['TagCloudJson'];
            var tgArr = getTreeFromJson(tagCloudJson);
            var arr = getTreeFromJson(json);
            var dict = result['Comments'];
            var sentences = result['Sentences'];
            var userComments = result['UserComments'];
            var keywordComments = result['KeywordComments'];
            var commentCount = result['TotalComments'];
            var allFollowing = result['AllFollowing'];
            var allWatching = result['AllWatching'];
            var totalWords = parseInt(result['TotalWords']);
            var totalSeconds = totalWords / 5;
            var MS_PER_SECOND = 1000;
            var endAt = new Date((new Date()).getTime() + (totalSeconds * MS_PER_SECOND));
            $("#divTimeSaved").countdown('destroy');
            $("#divTimeSaved").countdown({until:endAt,compact:true});
            if ($("#" + storyidval).length > 0) {
                var badge = $("#" + storyidval).find("span.pure-badge-info");
                badge.html(commentCount);
            }
            loadUserComments(userComments, storyidval,allFollowing);
            loadKeywordComments(keywordComments, storyidval,allWatching);
            loadSentencesForLoadStory(sentences);
            var arr2 = $.extend(true, {}, arr);
            loadTagCloudTree(tgArr);
            loadFullTree(arr);
            loadFullTree2(arr2);
            addLoadCommentEvent(dict);
            expandFullTree();
            displaySentencesDiv();
            //hide div for empty stories
            if (result['UserComments'].length === 0) {
                $("#divBody").hide();
                $("#divEmpty").show();
            } else {
                $("#divBody").show();
                $("#divEmpty").hide();
            }
        });
    } catch (ex) {
        alert(ex);
    }
}

function loadUserComments(comments,storyid,allFollowing) {
    var htmlUsers = '';
    var htmlUserComments = '';
    $("#hdnFollowing").html(allFollowing);
    $.each(comments, function (key, value) {
        var user = value['User'];
        var commentList = value['Comments'];
        var numComments = commentList.length;
        var styleInfo = "color:black";
        if (numComments >= 5) {
            styleInfo = "color:#00FF00;font-weight:bold";
        }
        if (numComments >= 10) {
            styleInfo = "color:red;font-weight:bold";
        }
        htmlUsers += "<li style='" + styleInfo + "'>" + user + "<span class='pure-badge-info'>" + numComments + "</span></li>";
        //<span class="pure-badge-info">{{>count}}</span>
        $.each(commentList, function (key1, value1) {
            htmlUserComments += "<li style='display:none;' id='" + value1['Id'] + ":" + storyid + ":" + user + "'>" + value1['Text'] + "<hr></li>";
        });
    });
    $("#selectable").html(htmlUsers);
    $("#selectableComment").html(htmlUserComments);
}

function refreshFollowingHiddenFieldList() {
    getFollowingFunction(-1, function (error, result) {
        try {
            if (error) console.log(error);
            $("#hdnFollowing").html(result.join(','));
        } catch (ex) {
            alert(ex.toString());
        }
    });
}

function refreshWatchingHiddenFieldList() {
    getWatchingFunction(-1, function (error, result) {
        try {
            if (error) console.log(error);
            $("#hdnWatching").html(result.join(','));
        } catch (ex) {
            alert(ex.toString());
        }
    });
}


function loadKeywordComments(comments, storyid, allWatching) {
    var htmlKeywords = '';
    $("#hdnWatching").html(allWatching);
    $.each(comments, function (key, value) {
        var keyword = value['Keyword'];
        var commentList = value['Comments'];
        var numComments = commentList.length;
        var styleInfo = "color:black";
        if (numComments >= 5) {
            styleInfo = "color:#00FF00;font-weight:bold";
        }
        if (numComments >= 10) {
            styleInfo = "color:red;font-weight:bold";
        }
        htmlKeywords += "<li style='" + styleInfo + "'>" + keyword + "<span class='pure-badge-info'>" + numComments + "</span></li>";
        $.each(commentList, function (key1, value1) {
            var li = $('<li />', {
                style: 'display:none',
                id: value1['Id'] + ":" + storyid + ":" + keyword,
                html: value1['Text']
            });
            var hr = $('<hr>');
            li.append(hr);
            $("#selectableKeywordComment").append(li);
        });
    });
    $("#selectableKeyword").html(htmlKeywords);
}


function loadTree(tree) {
    try {
        var tree1 = $("#treeDiv").fancytree('getTree');
        tree1.reload(tree);
    } catch (ex) {
        $("#treeDiv").fancytree({
            source: tree
        });
    }
    expandFullTree();
}

function loadTagCloudTree(tree) {
    try {
        var tree1 = $("#tagCloudTreeDiv").fancytree('getTree');
        tree1.reload(tree);
    } catch (ex) {
        $("#tagCloudTreeDiv").fancytree({
            source: tree,
            click:function(event, data) {
                var node = data.node;
                loadTreeByKey(node.key);
            }
        });
    }
}

function loadFullTree(tree) {
    try {
        var tree1 = $("#treeDiv").fancytree('getTree');
        tree1.reload(tree);
    } catch (ex) {
        $("#treeDiv").fancytree({
            source: tree
        });
    }
    expandFullTree();
}

function loadFullTree2(tree) {
    try {
        var tree2 = $("#hiddenTreeDiv").fancytree('getTree');
        tree2.reload(tree);
    } catch (ex) {
        $("#hiddenTreeDiv").fancytree({
            source: tree
        });
    }
}

function expandFullTree() {
    var treeNodes = $("#treeDiv").fancytree("getTree");
    treeNodes.visit(function (node) {
        node.setExpanded(true);
    });
    treeNodes.visit(function (node) {
        node.tooltip = htmlDecode("Parent: " + node.parent.data.text).substring(0, 100);
    });
}

function htmlEncode(value) {
    //create a in-memory div, set it's inner text(which jQuery automatically encodes)
    //then grab the encoded contents back out.  The div never exists on the page.
    return $('<div/>').text(value).html();
}

function htmlDecode(value) {
    return $('<div/>').html(value).text();
}

function clicked(item) {
    $("#commentDiv").html('');
    var storyidval = parseInt($(item).attr("id"));
    $("#hdnStoryId").html(storyidval);
    loadStory(storyidval);
    $("#list").children("div").children("div").each(function () {
        var element = $(this);
        if (element.hasClass("email-item-selected")) {
            element.removeClass("email-item-selected");
        }
    });
    $(item).addClass("email-item-selected");
}

function loadSentenceTreeWhenSentenceClicked(item) {
    var idtuple = $(item).attr("id");
    var key = idtuple.split(":")[0];
    $("#sentencesDiv").children("div").each(function() {
        var element = $(this);
        element.css('background-color','white');
    });
    $(item).css('background-color', 'lightgrey');
    loadTreeByKey(key);
}

function loadTreeForUserComment(idtuple) {
    var key = idtuple.split(":")[0];
    loadTreeByKey(key);
}

function deleteStory(item) {
    if (confirm('Are you sure?')) {
        var storyidval = parseInt($(item).attr("id").split('_')[1]);
        storyDeleteFunction(storyidval, function (error, result) {
            try {
                if (error) console.log(error);                
            } catch (ex) {
                alert(ex.toString());
            }
        });
        displayArchive();
    }
}

function getUpdates() {
    var ids = [];
    $('#stories .email-item').each(function() {
        ids.push(this.id);
    });
    var payload = {
        idList: ids
    };
    var stories = [];
    fetchFrontPage();
    checkForUpdatesFunction(payload, function (error, result) {
        try {
            if (error) console.log(error);
            var numNewStories = result.length;
            tempAlert("You have " + numNewStories + " new stories", 1000);
        } catch (ex) {
            alert(ex.toString());
        }
    });
}

function tempAlert(msg, duration) {
    var el = document.createElement("div");
    el.setAttribute("style", "position:absolute;top:40%;left:20%;background-color:white;");
    el.innerHTML = msg;
    setTimeout(function () {
        el.parentNode.removeChild(el);
    }, duration);
    document.body.appendChild(el);
}

function loadSentencesForLoadStory(sentences) {
    $("#sentencesDiv").html('');
    var k = 1;
    $.each(sentences, function (key, value) {
        var sentenceHtml = "<div style='word-wrap: break-word;' id='" + value['Id'] + ":" + value['StoryId'] + "' onclick='loadSentenceTreeWhenSentenceClicked(this);'><div id='hidden" + k + "' style='display: none;'>" + value['SentenceCommentTree'] + "</div><blockquote>" + value['Sentence'] + "<cite>" + value['Author'] + "</cite></blockquote></div>";
        $("#sentencesDiv").append(sentenceHtml);
        k++;
    });
}

function addLoadCommentEvent(dict) {
    var textdict = {};
    $.each(dict, function (key, value) {
        textdict[value['Id']] = value['Text'];
    });
    for (var property in textdict) {
        if (textdict.hasOwnProperty(property)) {

        }
    }
}

function loadComment(textdict, key) {
    $("#commentDiv").html('');
    $("#commentDiv").html(textdict[key]);
}

function openurl(url) {
    var gui = require('nw.gui');
    gui.Shell.openExternal(url);
    if (!e) var e = window.event;
    e.cancelBubble = true;
    if (e.stopPropagation) e.stopPropagation();
}