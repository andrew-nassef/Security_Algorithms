#include<iostream>
#include<string>
#include<vector>

using namespace std;


void backtrack(vector<int>s,int size, vector<vector<int>> &result) {
	if (s.size() == size) {
		for (int i = 0; i < size; i++) {
			for (int j = i + 1; j < size; j++) {
				if (s[i] == s[j])return;
			}
		}
		result.push_back(s);
		return;
	}
	for (int i = 1; i <= size; i++) {
		s.push_back(i);
		backtrack(s,size, result);
		s.pop_back();
	}
}

int main() {
	vector<vector<int>> result;
	vector<int>s;
	backtrack(s,7, result);
	for (int i = 0; i < result.size(); i++) {
		for (int j = 0; j < 7; j++) {
			cout << result[i][j];
		}
		cout << endl;
	}
	return 0;
}