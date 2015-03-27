function displaySentencesDiv() {
    $("#sentencesDiv").show();
    $("#usersDiv").hide();
    $("#keywordsDiv").hide();
    $("#btnUp").show();
}

function displayUsersDiv() {
    $("#sentencesDiv").hide();
    $("#usersDiv").show();
    $("#keywordsDiv").hide();
}

function displayKeywordsDiv() {
    $("#sentencesDiv").hide();
    $("#usersDiv").hide();
    $("#keywordsDiv").show();
}

function up() {
    console.log('inside up');
    var currRootNode = $("#treeDiv").fancytree("getTree").getRootNode().getFirstChild();
    console.log(currRootNode);
    var currRootNodeKey = currRootNode.key;
    console.log(currRootNodeKey);
    var rootNodeHidden = $("#hiddenTreeDiv").fancytree("getTree").getNodeByKey(currRootNodeKey);
    console.log(rootNodeHidden);
    var parentNode = rootNodeHidden.getParent();
    console.log(parentNode);
    var parentKey = parentNode.key;
    console.log(parentKey);
    loadTreeByKey(parentKey);
}

function loadTreeByKey(key) {
    console.log('inside loadtreebykey');
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

function displayStories() {
    var stories = [];
    top100Function(100, function (error, result) {
        try {
            if (error) console.log(error);
            result.forEach(function (entry) {
                var story = { author: entry['Author'], title: entry['StoryTitle'], text: entry['StoryText'] ? entry['StoryText'].substring(1, 100) : '', storyid: entry['StoryId'], storyurl: entry['Url'], hdnstoryid: 'hdn' + entry['StoryId'], count: entry['StoryComments'], commentUrl: entry['CommentUrl'], storyurlwithquotes: '\'' + entry['Url'] + '\'', commenturlwithquotes: '\'' + entry['CommentUrl'] + '\'' };
                stories.push(story);
            });
        } catch (ex) {
            alert(ex.toString());
        }
    });
    render(stories);
}

function displayArchive() {
    var stories = [];
    getArchiveFunction(100, function (error, result) {
        try {
            if (error) console.log(error);
            result.forEach(function (entry) {
                var story = { author: entry['Author'], title: entry['StoryTitle'], text: entry['StoryText'] ? entry['StoryText'].substring(1, 100) : '', storyid: entry['StoryId'], storyurl: entry['Url'], hdnstoryid: 'hdn' + entry['StoryId'], count: entry['StoryComments'], commentUrl: entry['CommentUrl'], storyurlwithquotes: '\'' + entry['Url'] + '\'', commenturlwithquotes: '\'' + entry['CommentUrl'] + '\'' };
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

function getStory() {
    try {
        var stories = [];
        var storytext = $('#inputStoryId').val();
        var storyidtext = storytext.split('=')[1];
        var storyidval = parseInt(storyidtext);
        storyFunction(storyidval, function (error, result) {
            try {
                if (error) console.log(error);
                result.forEach(function (entry) {
                    var story = { author: entry['Author'], title: entry['StoryTitle'], text: entry['StoryText'] ? entry['StoryText'].substring(1, 100) : '', storyid: entry['StoryId'], storyurl: entry['Url'], hdnstoryid: 'hdn' + entry['StoryId'], count: entry['StoryComments'], commentUrl: entry['CommentUrl'], storyurlwithquotes: '\'' + entry['Url'] + '\'', commenturlwithquotes: '\'' + entry['CommentUrl'] + '\'' };
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

function archiveStory() {
    var storyidval = parseInt($("#hdnStoryId").html());
    archive(storyidval);
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
        tagCloudFunction(storyidval, function (error, result) {
            if (error) console.log('error:' + error);
            var json = result['Json'];
            var arr = getTreeFromJson(json);
            var dict = result['Comments'];
            var sentences = result['Sentences'];
            var userComments = result['UserComments'];
            var keywordComments = result['KeywordComments'];
            loadUserComments(userComments, storyidval);
            loadKeywordComments(keywordComments, storyidval);
            loadSentences(sentences);
            var arr2 = $.extend(true, {}, arr);
            loadFullTree(arr);
            loadFullTree2(arr2);
            addLoadCommentEvent(dict);
            expandFullTree();
            displaySentencesDiv();
        });
    } catch (ex) {
        alert(ex);
    }
}

function loadUserComments(comments,storyid) {
    var htmlUsers = '';
    var htmlUserComments = '';
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
        htmlUsers += "<li style='"+styleInfo+"'>" + user + "<span class='pure-badge-info'>"+numComments+"</span></li>";
        //<span class="pure-badge-info">{{>count}}</span>
        $.each(commentList, function(key1, value1) {
            htmlUserComments += "<li style='display:none;' id='"+value1['Id']+ ":"+storyid+":"+user+"'>"+ value1['Text'] + "<hr></li>";
        });
    });
    $("#selectable").html(htmlUsers);
    $("#selectableComment").html(htmlUserComments);
}


function loadKeywordComments(comments, storyid) {
    var htmlUsers = '';
    var htmlUserComments = '';
    $.each(comments, function (key, value) {
        var user = value['Keyword'];
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
    $("#selectableKeyword").html(htmlUsers);
    $("#selectableKeywordComment").html(htmlUserComments);
}


function loadTree(tree) {
    console.log(tree);
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

function loadFullTree(tree) {
    console.log(tree);
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
    console.log(tree);
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
        node.tooltip = htmlDecode("Parent: " + node.parent.data.text).substring(0, 100);
    });
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
        console.log("inside the list div for div = " + element.attr("id"));
        if (element.hasClass("email-item-selected")) {
            element.removeClass("email-item-selected");
        }
    });
    $(item).addClass("email-item-selected");
}

function loadSentenceTree(item) {
    var idtuple = $(item).attr("id");
    console.log(idtuple);
    var key = idtuple.split(":")[0];
    console.log(key);
    loadTreeByKey(key);
}

function loadTreeForUserComment(idtuple) {
    console.log(idtuple);
    var key = idtuple.split(":")[0];
    console.log(key);
    loadTreeByKey(key);
}

function expandFullTree() {
    var treeNodes = $("#treeDiv").fancytree("getTree");
    treeNodes.visit(function (node) {
        node.setExpanded(true);
    });
}

function loadSentences(sentences) {
    $("#sentencesDiv").html('');
    var k = 1;
    $.each(sentences, function (key, value) {
        var sentenceHtml = "<div style='word-wrap: break-word;' id='" + value['Id'] + ":" + value['StoryId'] + "' onclick='loadSentenceTree(this);'><div id='hidden" + k + "' style='display: none;'>" + value['SentenceCommentTree'] + "</div><blockquote>" + value['Sentence'] + "<cite>" + value['Author'] + "</cite></blockquote></div>";
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