mergeInto(LibraryManager.library, {

	GetRanking: function () {
		var ranking = "Ranking\n";
		var db = firebase.firestore();
		var cnt = 0;
		db.collection("users").limit(10).orderBy("time").get().then(function(querySnapshot) {
			querySnapshot.forEach(function(doc) {
				cnt++;
				// doc.data() is never undefined for query doc snapshots
				var data = doc.data();
				ranking = ranking + cnt + "." + doc.id + " " + data.time + "s\n";
				// console.log(cnt + "ranking : " + ranking);
				SendMessage('Ranking', 'UpdateRanking', ranking);
			});
		});
	},
	SetRanking: function(name, time) {
		name = Pointer_stringify(name);
		time = parseFloat(Pointer_stringify(time));
		var db = firebase.firestore();
		var docRef = db.collection("users").doc(name);
		docRef.get().then(function(doc) {
			if (doc.exists) { //ある
				var data = doc.data();
				console.log(data.time + " > " + time);
				if (data.time > time) {
					db.collection("users").doc(name).set({
						time: time
					});
				}
			} else { //ない
				db.collection("users").doc(name).set({
					time: time
				});
			}
		}).catch(function(error) {
			console.log("Error getting document:", error);
		});
	},
});