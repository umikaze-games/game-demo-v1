﻿using System.Collections.Generic;
using UnityEngine;

public class AnimatorOverride : MonoBehaviour
{
	private Animator[] animators;
	public SpriteRenderer holdItem;

	public List<AnimatorType> animatorTypes;

	private Dictionary<string, Animator> animatorNameDict = new Dictionary<string, Animator>();

	private void Awake()
	{
		animators = GetComponentsInChildren<Animator>();

		foreach (Animator animator in animators)
		{
			animatorNameDict.Add(animator.name, animator);
			Debug.Log("Adding animator: " + animator.name);
		}
	}

	private void OnEnable()
	{
		EventHandler.ItemSelectedEvent += OnItemSelectedEvent;
	}
	private void OnDisable()
	{
		EventHandler.ItemSelectedEvent -= OnItemSelectedEvent;
	}

	private void OnItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
	{
		PartType currentType = itemDetails.itemType switch
		{
			ItemType.Seed => PartType.Carry,
			ItemType.Product => PartType.Carry,
			_ => PartType.None
		};

		if (isSelected == false)
		{
			currentType = PartType.None;
			holdItem.enabled = false;
		}
		if (currentType==PartType.Carry)
		{
			holdItem.sprite = itemDetails.itemOnWorldSprite;
			holdItem.enabled = true;
		}

		SwitchAnimator(currentType);
	}

	private void SwitchAnimator(PartType partType)
	{
		foreach (var item in animatorTypes)
		{
			if (item.partType == partType)
			{
				animatorNameDict[item.partName.ToString()].runtimeAnimatorController = item.overrideController;
			}
		}
	}

}